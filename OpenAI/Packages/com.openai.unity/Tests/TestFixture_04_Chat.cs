// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Chat;
using OpenAI.Models;
using OpenAI.Tests.StructuredOutput;
using OpenAI.Tests.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_04_Chat : AbstractTestFixture
    {
        [Test]
        public async Task Test_01_01_GetChatCompletion()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Who won the world series in 2020?"),
                new(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new(Role.User, "Where was it played?")
            };
            var chatRequest = new ChatRequest(messages, Model.GPT4o);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);
            Assert.AreEqual(1, response.Choices.Count);
            Assert.IsNotNull(response.FirstChoice);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");
            response.GetUsage();
        }

        [Test]
        public async Task Test_01_02_GetChatStreamingCompletion()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Who won the world series in 2020?"),
                new(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new(Role.User, "Where was it played?")
            };
            var chatRequest = new ChatRequest(messages);
            var cumulativeDelta = string.Empty;
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    cumulativeDelta += choice.Delta.Content;
                }
            }, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            var choice = response.FirstChoice;
            Assert.IsNotNull(choice);
            Assert.IsNotNull(choice.Message);
            Assert.IsFalse(string.IsNullOrEmpty(choice.ToString()));
            Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice} | Finish Reason: {choice.FinishReason}");
            Assert.IsTrue(choice.Message.Role == Role.Assistant);
            Assert.IsTrue(choice.Message.Content!.Equals(cumulativeDelta));
            Debug.Log(response.ToString());
            response.GetUsage();
        }

        [Test]
        public async Task Test_01_03_01_GetChatCompletion_Modalities()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);

            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Is a golden retriever a good family dog?"),
            };

            var chatRequest = new ChatRequest(messages, Model.GPT4oAudio, audioConfig: Voice.Alloy);
            Assert.IsNotNull(chatRequest);
            Assert.IsNotNull(chatRequest.AudioConfig);
            Assert.AreEqual(Model.GPT4oAudio.Id, chatRequest.Model);
            Assert.AreEqual(Voice.Alloy.Id, chatRequest.AudioConfig.Voice);
            Assert.AreEqual(AudioFormat.Pcm16, chatRequest.AudioConfig.Format);
            Assert.AreEqual(Modality.Text | Modality.Audio, chatRequest.Modalities);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);
            Assert.AreEqual(1, response.Choices.Count);
            Assert.IsNotNull(response.FirstChoice);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");
            Assert.IsNotNull(response.FirstChoice.Message.AudioOutput.AudioClip);
            response.GetUsage();

            messages.Add(response.FirstChoice.Message);
            messages.Add(new(Role.User, "What are some other good family dog breeds?"));

            chatRequest = new ChatRequest(messages, Model.GPT4oAudio, audioConfig: Voice.Alloy);
            Assert.IsNotNull(chatRequest);
            Assert.IsNotNull(messages[2]);
            Assert.AreEqual(Role.Assistant, messages[2].Role);
            Assert.IsNotNull(messages[2].AudioOutput);
            response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);
            Assert.AreEqual(1, response.Choices.Count);
            Assert.IsNotNull(response.FirstChoice);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");
            Assert.IsNotNull(response.FirstChoice.Message.AudioOutput.AudioClip);
            response.GetUsage();
        }

        [Test]
        public async Task Test_01_03_02_GetChatCompletion_Modalities_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Is a golden retriever a good family dog?"),
            };
            var chatRequest = new ChatRequest(messages, Model.GPT4oAudio, audioConfig: Voice.Alloy);
            Assert.IsNotNull(chatRequest);
            Assert.IsNotNull(chatRequest.AudioConfig);
            Assert.AreEqual(Model.GPT4oAudio.Id, chatRequest.Model);
            Assert.AreEqual(Voice.Alloy.Id, chatRequest.AudioConfig.Voice);
            Assert.AreEqual(AudioFormat.Pcm16, chatRequest.AudioConfig.Format);
            Assert.AreEqual(Modality.Text | Modality.Audio, chatRequest.Modalities);
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, Assert.IsNotNull, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);
            Assert.AreEqual(1, response.Choices.Count);
            Assert.IsNotNull(response.FirstChoice);
            Console.WriteLine($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");
            Assert.IsNotEmpty(response.FirstChoice.Message.AudioOutput.Transcript);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.FirstChoice.Message.AudioOutput.Data));
            response.GetUsage();
            messages.Add(response.FirstChoice.Message);
            messages.Add(new(Role.User, "What are some other good family dog breeds?"));
            chatRequest = new ChatRequest(messages, Model.GPT4oAudio, audioConfig: Voice.Alloy);
            Assert.IsNotNull(chatRequest);
            Assert.IsNotNull(messages[2]);
            Assert.AreEqual(Role.Assistant, messages[2].Role);
            Assert.IsNotNull(messages[2].AudioOutput);
            response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, Assert.IsNotNull, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);
            Assert.AreEqual(1, response.Choices.Count);
            Assert.IsNotEmpty(response.FirstChoice.Message.AudioOutput.Transcript);
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.FirstChoice.Message.AudioOutput.Data));
            Assert.IsFalse(string.IsNullOrWhiteSpace(response.FirstChoice));
        }

        [Test]
        public async Task Test_01_03_JsonMode()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant designed to output JSON."),
                new(Role.User, "Who won the world series in 2020?"),
            };
            var chatRequest = new ChatRequest(messages, Model.GPT4o, responseFormat: ChatResponseFormat.Json);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);

            foreach (var choice in response.Choices)
            {
                Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice} | Finish Reason: {choice.FinishReason}");
            }

            response.GetUsage();
        }

        [Test]
        public async Task Test_02_01_GetChatToolCompletion()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);

            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful weather assistant. Always ask the user for their location."),
                new(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = new List<Tool>
            {
                Tool.GetOrCreateTool(typeof(WeatherService), nameof(WeatherService.GetCurrentWeatherAsync))
            };
            Assert.IsTrue(tools.All(tool => tool.Function?.Arguments == null));
            var chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "none");
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            Assert.IsTrue(response.FirstChoice.FinishReason == "stop");
            messages.Add(response.FirstChoice.Message);

            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            messages.Add(response.FirstChoice.Message);

            if (response.FirstChoice.FinishReason == "stop")
            {
                Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "Fahrenheit");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
                response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Choices);
                Assert.IsTrue(response.Choices.Count == 1);
            }

            Assert.IsTrue(response.FirstChoice.FinishReason == "tool_calls");
            Assert.IsTrue(response.FirstChoice.Message.ToolCalls.Count == 1);
            var usedTool = response.FirstChoice.Message.ToolCalls[0];
            Assert.IsNotNull(usedTool);
            Assert.IsTrue(usedTool.Function.Name.Contains(nameof(WeatherService.GetCurrentWeatherAsync)));
            Debug.Log($"{response.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {response.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");
            var functionResult = await usedTool.InvokeFunctionAsync();
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");
            chatRequest = new ChatRequest(messages);
            response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_02_02_GetChatToolCompletion_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful weather assistant. Always prompt the user for their location."),
                new(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = new List<Tool>
            {
                Tool.GetOrCreateTool(typeof(WeatherService), nameof(WeatherService.GetCurrentWeatherAsync))
            };
            Assert.IsTrue(tools.All(tool => tool.Function?.Arguments == null));
            var chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "none");
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            }, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            messages.Add(response.FirstChoice.Message);

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            }, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            messages.Add(response.FirstChoice.Message);

            if (response.FirstChoice.FinishReason == "stop")
            {
                Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "Fahrenheit");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
                response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
                {
                    Assert.IsNotNull(partialResponse);
                    if (partialResponse.Usage != null) { return; }
                    Assert.NotNull(partialResponse.Choices);
                    Assert.NotZero(partialResponse.Choices.Count);
                }, true);
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Choices);
                Assert.IsTrue(response.Choices.Count == 1);
            }

            Assert.IsTrue(response.FirstChoice.FinishReason == "tool_calls");
            Assert.IsTrue(response.FirstChoice.Message.ToolCalls.Count == 1);
            var usedTool = response.FirstChoice.Message.ToolCalls[0];
            Assert.IsNotNull(usedTool);
            Assert.IsTrue(usedTool.Function.Name.Contains(nameof(WeatherService.GetCurrentWeatherAsync)));
            Debug.Log($"{response.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {response.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");
            var functionResult = await usedTool.InvokeFunctionAsync();
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");

            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "none");
            response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            }, true);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Test_02_03_ChatCompletion_Multiple_Tools_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful weather assistant. Use the appropriate unit based on geographical location."),
                new(Role.User, "What's the weather like today in Los Angeles, USA and Tokyo, Japan?"),
            };

            var tools = Tool.GetAllAvailableTools(false, forceUpdate: true, clearCache: true);
            Assert.IsTrue(tools.All(tool => tool.Function?.Arguments == null));
            var chatRequest = new ChatRequest(messages, model: Model.GPT4o, tools: tools, toolChoice: "auto", parallelToolCalls: true);
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            }, true);

            Assert.IsTrue(response.FirstChoice.FinishReason == "tool_calls");
            messages.Add(response.FirstChoice.Message);

            var toolCalls = response.FirstChoice.Message.ToolCalls;

            Assert.NotNull(toolCalls);
            Assert.AreEqual(2, toolCalls.Count);

            foreach (var toolCall in toolCalls)
            {
                var output = await toolCall.InvokeFunctionAsync();
                messages.Add(new Message(toolCall, output));
            }

            chatRequest = new ChatRequest(messages, model: Model.GPT4o, tools: tools, toolChoice: "none");
            response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Test_02_04_GetChatToolForceCompletion()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful weather assistant. Use the appropriate unit based on geographical location."),
                new(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = Tool.GetAllAvailableTools(false, forceUpdate: true, clearCache: true);
            Assert.IsTrue(tools.All(tool => tool.Function?.Arguments == null));
            var chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "none");
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            Assert.IsTrue(response.FirstChoice.FinishReason == "stop");
            messages.Add(response.FirstChoice.Message);

            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in New York, USA");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(
                messages,
                tools: tools,
                toolChoice: nameof(WeatherService.GetCurrentWeatherAsync));
            response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == 1);
            messages.Add(response.FirstChoice.Message);

            Assert.IsTrue(response.FirstChoice.FinishReason == "stop");
            Assert.IsTrue(response.FirstChoice.Message.ToolCalls.Count == 1);
            var usedTool = response.FirstChoice.Message.ToolCalls[0];
            Assert.IsNotNull(usedTool);
            Assert.IsTrue(usedTool.Function.Name.Contains(nameof(WeatherService.GetCurrentWeatherAsync)));
            Debug.Log($"{response.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {response.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");
            var functionResult = await usedTool.InvokeFunctionAsync();
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");
        }

        [Test]
        public async Task Test_03_01_GetChatVision()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, new List<Content>
                {
                    "What's in this image?",
                    new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg", ImageDetail.Low)
                })
            };
            var chatRequest = new ChatRequest(messages, model: Model.GPT4o);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishDetails}");
            response.GetUsage();
        }

        [Test]
        public async Task Test_03_02_GetChatVisionStreaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, new List<Content>
                {
                    "What's in this image?",
                    new ImageUrl("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg", ImageDetail.Low)
                })
            };
            var chatRequest = new ChatRequest(messages, model: Model.GPT4o);
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            }, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishDetails}");
            response.GetUsage();
        }

        [Test]
        public async Task Test_03_03_GetChatVision_Texture()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);

            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, new List<Content>
                {
                    "What's in this image?",
                    image
                })
            };

            var chatRequest = new ChatRequest(messages, model: Model.GPT4o);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Debug.Log($"{response.FirstChoice.Message.Role}: {response.FirstChoice} | Finish Reason: {response.FirstChoice.FinishDetails}");
            response.GetUsage();
        }

        [Test]
        public async Task Test_04_01_GetChatLogProbs()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Who won the world series in 2020?"),
                new(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages, topLogProbs: 1);
            var response = await OpenAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsNotEmpty(response.Choices);

            foreach (var choice in response.Choices)
            {
                Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice} | Finish Reason: {choice.FinishReason}");
            }

            response.GetUsage();
        }

        [Test]
        public async Task Test_04_02_GetChatLogProbsStreaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);
            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful assistant."),
                new(Role.User, "Who won the world series in 2020?"),
                new(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages, topLogProbs: 1);
            var cumulativeDelta = string.Empty;
            var response = await OpenAIClient.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    cumulativeDelta += choice.Delta.Content;
                }
            }, true);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            var choice = response.FirstChoice;
            Assert.IsNotNull(choice);
            Assert.IsNotNull(choice.Message);
            Assert.IsFalse(string.IsNullOrEmpty(choice.ToString()));
            Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice} | Finish Reason: {choice.FinishReason}");
            Assert.IsTrue(choice.Message.Role == Role.Assistant);
            Assert.IsTrue(choice.Message.Content!.Equals(cumulativeDelta));
            Debug.Log(response.ToString());
            response.GetUsage();
        }

        [Test]
        public async Task Test_05_01_GetChat_JsonSchema()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);

            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful math tutor. Guide the user through the solution step by step."),
                new(Role.User, "how can I solve 8x + 7 = -23")
            };
            var chatRequest = new ChatRequest(messages, model: "gpt-4o-2024-08-06");
            var (mathResponse, chatResponse) = await OpenAIClient.ChatEndpoint.GetCompletionAsync<MathResponse>(chatRequest);
            Assert.IsNotNull(chatResponse);
            Assert.IsNotNull(mathResponse);
            Assert.IsNotEmpty(mathResponse.Steps);
            Assert.IsNotNull(chatResponse.Choices);
            Assert.IsNotEmpty(chatResponse.Choices);

            for (var i = 0; i < mathResponse.Steps.Count; i++)
            {
                var step = mathResponse.Steps[i];
                Assert.IsNotNull(step.Explanation);
                Debug.Log($"Step {i}: {step.Explanation}");
                Assert.IsNotNull(step.Output);
                Debug.Log($"Result: {step.Output}");
            }

            Assert.IsNotNull(mathResponse.FinalAnswer);
            Debug.Log($"Final Answer: {mathResponse.FinalAnswer}");

            chatResponse.GetUsage();
        }

        [Test]
        public async Task Test_05_02_GetChat_JsonSchema_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.ChatEndpoint);

            var messages = new List<Message>
            {
                new(Role.System, "You are a helpful math tutor. Guide the user through the solution step by step."),
                new(Role.User, "how can I solve 8x + 7 = -23")
            };
            var chatRequest = new ChatRequest(messages, model: "gpt-4o-2024-08-06");
            var cumulativeDelta = string.Empty;
            var (mathResponse, chatResponse) = await OpenAIClient.ChatEndpoint.StreamCompletionAsync<MathResponse>(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                if (partialResponse.Usage != null) { return; }
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    cumulativeDelta += choice.Delta.Content;
                }
            }, true);
            Assert.IsNotNull(chatResponse);
            Assert.IsNotNull(mathResponse);
            Assert.IsNotNull(chatResponse.Choices);
            var choice = chatResponse.FirstChoice;
            Assert.IsNotNull(choice);
            Assert.IsNotNull(choice.Message);
            Assert.IsFalse(string.IsNullOrEmpty(choice.ToString()));
            Assert.IsTrue(choice.Message.Role == Role.Assistant);
            Assert.IsTrue(choice.Message.Content!.Equals(cumulativeDelta));

            for (var i = 0; i < mathResponse.Steps.Count; i++)
            {
                var step = mathResponse.Steps[i];
                Assert.IsNotNull(step.Explanation);
                Debug.Log($"Step {i}: {step.Explanation}");
                Assert.IsNotNull(step.Output);
                Debug.Log($"Result: {step.Output}");
            }

            Assert.IsNotNull(mathResponse.FinalAnswer);
            Debug.Log($"Final Answer: {mathResponse.FinalAnswer}");

            chatResponse.GetUsage();
        }
    }
}
