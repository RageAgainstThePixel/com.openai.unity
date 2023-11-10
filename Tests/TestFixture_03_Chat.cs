// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenAI.Chat;
using OpenAI.Tests.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Tool = OpenAI.Chat.Tool;

namespace OpenAI.Tests
{
    internal class TestFixture_03_Chat
    {
        [Test]
        public async Task Test_01_GetChatCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, "Who won the world series in 2020?"),
                new Message(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new Message(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages, number: 2);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 2);

            foreach (var choice in result.Choices)
            {
                Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content} | Finish Reason: {choice.FinishReason}");
            }

            result.GetUsage();
        }

        [Test]
        public async Task Test_02_GetChatStreamingCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            const int choiceCount = 2;
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, "Who won the world series in 2020?"),
                new Message(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new Message(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages, number: choiceCount);
            var cumulativeDelta = new List<string>();

            for (var i = 0; i < choiceCount; i++)
            {
                cumulativeDelta.Add(string.Empty);
            }

            var response = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    Debug.Log($"[{choice.Index}] {choice.Delta.Content}");
                    cumulativeDelta[choice.Index] += choice.Delta.Content;
                }
            });

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Choices);
            Assert.IsTrue(response.Choices.Count == choiceCount);

            for (var i = 0; i < choiceCount; i++)
            {
                var choice = response.Choices[i];
                Assert.IsFalse(string.IsNullOrEmpty(choice?.Message?.ToString()));
                Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content} | Finish Reason: {choice.FinishReason}");
                Assert.IsTrue(choice.Message.Role == Role.Assistant);
                var deltaContent = cumulativeDelta[i];
                Assert.IsTrue(choice.Message.Content.Equals(deltaContent));
            }

            response.GetUsage();
        }

        [Test]
        [Obsolete]
        public async Task Test_04_GetChatFunctionCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var functions = new List<Function>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                     new JObject
                     {
                         ["type"] = "object",
                         ["properties"] = new JObject
                         {
                             ["location"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["description"] = "The city and state, e.g. San Francisco, CA"
                             },
                             ["unit"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["enum"] = new JArray {"celsius", "fahrenheit"}
                             }
                         },
                         ["required"] = new JArray { "location", "unit" }
                     })
            };

            var chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            if (!string.IsNullOrEmpty(result.FirstChoice.Message.ToString()))
            {
                Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "celsius");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
                result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Choices);
                Assert.IsTrue(result.Choices.Count == 1);
            }

            Assert.IsTrue(result.FirstChoice.FinishReason == "function_call");
            Assert.IsTrue(result.FirstChoice.Message.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{result.FirstChoice.Message.Function.Arguments}");

            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(result.FirstChoice.Message.Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(Role.Function, functionResult, nameof(WeatherService.GetCurrentWeather)));
            Debug.Log($"{Role.Function}: {functionResult}");
            chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Debug.Log(result);
        }

        [Test]
        [Obsolete]
        public async Task Test_05_GetChatFunctionCompletion_Streaming()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var functions = new List<Function>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                    new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JObject
                        {
                            ["location"] = new JObject
                            {
                                ["type"] = "string",
                                ["description"] = "The city and state, e.g. San Francisco, CA"
                            },
                            ["unit"] = new JObject
                            {
                                ["type"] = "string",
                                ["enum"] = new JArray {"celsius", "fahrenheit"}
                            }
                        },
                        ["required"] = new JArray { "location", "unit" }
                    })
            };

            var chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
            var result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Delta?.Content)))
                {
                    Debug.Log($"{choice.Delta.Content}");
                }

                foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Message?.Content?.ToString())))
                {
                    Debug.Log($"{choice.Message.Role}: {choice.Message.Content} | Finish Reason: {choice.FinishReason}");
                }
            });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
            result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);

                foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Delta?.Content)))
                {
                    Debug.Log($"[{choice.Index}] {choice.Delta.Content}");
                }

                foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Message?.Content?.ToString())))
                {
                    Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content} | Finish Reason: {choice.FinishReason}");
                }
            });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            if (!string.IsNullOrEmpty(result.FirstChoice.Message?.ToString()))
            {
                Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "celsius");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, functions: functions, functionCall: "auto");
                result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
                {
                    Assert.IsNotNull(partialResponse);
                    Assert.NotNull(partialResponse.Choices);
                    Assert.NotZero(partialResponse.Choices.Count);

                    foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Delta?.Content)))
                    {
                        Debug.Log($"{choice.Delta.Content}");
                    }

                    foreach (var choice in partialResponse.Choices.Where(choice => !string.IsNullOrEmpty(choice.Message?.ToString())))
                    {
                        Debug.Log($"{choice.Message.Role}: {choice.Message.Content} | Finish Reason: {choice.FinishReason}");
                    }
                });
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Choices);
                Assert.IsTrue(result.Choices.Count == 1);
            }

            Assert.IsTrue(result.FirstChoice.FinishReason == "function_call");
            Assert.IsTrue(result.FirstChoice.Message.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{result.FirstChoice.Message.Function.Arguments}");

            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(result.FirstChoice.Message.Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(Role.Function, functionResult, nameof(WeatherService.GetCurrentWeather)));
            Debug.Log($"{Role.Function}: {functionResult}");
        }

        [Test]
        [Obsolete]
        public async Task Test_06_GetChatFunctionForceCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var functions = new List<Function>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                     new JObject
                     {
                         ["type"] = "object",
                         ["properties"] = new JObject
                         {
                             ["location"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["description"] = "The city and state, e.g. San Francisco, CA"
                             },
                             ["unit"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["enum"] = new JArray {"celsius", "fahrenheit"}
                             }
                         },
                         ["required"] = new JArray { "location", "unit" }
                     })
            };

            var chatRequest = new ChatRequest(messages, functions: functions);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(
                messages,
                functions: functions,
                functionCall: nameof(WeatherService.GetCurrentWeather),
                model: "gpt-3.5-turbo");
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            Assert.IsTrue(result.FirstChoice.FinishReason == "stop");
            Assert.IsTrue(result.FirstChoice.Message.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{result.FirstChoice.Message.Function.Arguments}");

            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(result.FirstChoice.Message.Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(Role.Function, functionResult, nameof(WeatherService.GetCurrentWeather)));
            Debug.Log($"{Role.Function}: {functionResult}");
        }

        [Test]
        public async Task Test_07_GetChatToolCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);

            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = new List<Tool>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                     new JObject
                     {
                         ["type"] = "object",
                         ["properties"] = new JObject
                         {
                             ["location"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["description"] = "The city and state, e.g. San Francisco, CA"
                             },
                             ["unit"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["enum"] = new JArray {"celsius", "fahrenheit"}
                             }
                         },
                         ["required"] = new JArray { "location", "unit" }
                     })
            };

            var chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            if (!string.IsNullOrEmpty(result.FirstChoice.Message.ToString()))
            {
                Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "celsius");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
                result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Choices);
                Assert.IsTrue(result.Choices.Count == 1);
            }

            var usedTool = result.FirstChoice.Message.ToolCalls[0];
            Assert.IsTrue(result.FirstChoice.FinishReason == "tool_calls");
            Assert.IsTrue(usedTool.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");
            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(usedTool.Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");
            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_08_GetChatToolCompletion_Streaming()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = new List<Tool>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                    new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JObject
                        {
                            ["location"] = new JObject
                            {
                                ["type"] = "string",
                                ["description"] = "The city and state, e.g. San Francisco, CA"
                            },
                            ["unit"] = new JObject
                            {
                                ["type"] = "string",
                                ["enum"] = new JArray {"celsius", "fahrenheit"}
                            }
                        },
                        ["required"] = new JArray { "location", "unit" }
                    })
            };

            var chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            var result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
            result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            if (!string.IsNullOrEmpty(result.FirstChoice.Message.ToString()))
            {
                Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

                var unitMessage = new Message(Role.User, "celsius");
                messages.Add(unitMessage);
                Debug.Log($"{unitMessage.Role}: {unitMessage.Content}");
                chatRequest = new ChatRequest(messages, tools: tools, toolChoice: "auto");
                result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
                {
                    Assert.IsNotNull(partialResponse);
                    Assert.NotNull(partialResponse.Choices);
                    Assert.NotZero(partialResponse.Choices.Count);
                });
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Choices);
                Assert.IsTrue(result.Choices.Count == 1);
            }

            Assert.IsTrue(result.FirstChoice.FinishReason == "tool_calls");
            var usedTool = result.FirstChoice.Message.ToolCalls[0];
            Assert.IsTrue(usedTool.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");

            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(usedTool.Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");
        }

        [Test]
        public async Task Test_09_GetChatToolForceCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful weather assistant."),
                new Message(Role.User, "What's the weather like today?"),
            };

            foreach (var message in messages)
            {
                Debug.Log($"{message.Role}: {message.Content}");
            }

            var tools = new List<Tool>
            {
                new Function(
                    nameof(WeatherService.GetCurrentWeather),
                    "Get the current weather in a given location",
                     new JObject
                     {
                         ["type"] = "object",
                         ["properties"] = new JObject
                         {
                             ["location"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["description"] = "The city and state, e.g. San Francisco, CA"
                             },
                             ["unit"] = new JObject
                             {
                                 ["type"] = "string",
                                 ["enum"] = new JArray {"celsius", "fahrenheit"}
                             }
                         },
                         ["required"] = new JArray { "location", "unit" }
                     })
            };

            var chatRequest = new ChatRequest(messages, tools: tools);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishReason}");

            var locationMessage = new Message(Role.User, "I'm in Glasgow, Scotland");
            messages.Add(locationMessage);
            Debug.Log($"{locationMessage.Role}: {locationMessage.Content}");
            chatRequest = new ChatRequest(
                messages,
                tools: tools,
                toolChoice: nameof(WeatherService.GetCurrentWeather));
            result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Assert.IsTrue(result.Choices.Count == 1);
            messages.Add(result.FirstChoice.Message);

            var usedTool = result.FirstChoice.Message.ToolCalls[0];
            Assert.IsTrue(result.FirstChoice.FinishReason == "stop");
            Assert.IsTrue(usedTool.Function.Name == nameof(WeatherService.GetCurrentWeather));
            Debug.Log($"{result.FirstChoice.Message.Role}: {usedTool.Function.Name} | Finish Reason: {result.FirstChoice.FinishReason}");
            Debug.Log($"{usedTool.Function.Arguments}");
            var functionArgs = JsonConvert.DeserializeObject<WeatherArgs>(result.FirstChoice.Message.ToolCalls[0].Function.Arguments.ToString());
            var functionResult = WeatherService.GetCurrentWeather(functionArgs);
            Assert.IsNotNull(functionResult);
            messages.Add(new Message(usedTool, functionResult));
            Debug.Log($"{Role.Tool}: {functionResult}");
        }

        [Test]
        public async Task Test_10_GetChatVision()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, new List<Content>
                {
                    new Content(ContentType.Text, "What's in this image?"),
                    new Content(ContentType.ImageUrl, "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg")
                })
            };
            var chatRequest = new ChatRequest(messages, model: "gpt-4-vision-preview");
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishDetails}");
            result.GetUsage();
        }

        [Test]
        public async Task Test_11_GetChatVisionStreaming()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, new List<Content>
                {
                    new Content(ContentType.Text, "What's in this image?"),
                    new Content(ContentType.ImageUrl, "https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg")
                })
            };
            var chatRequest = new ChatRequest(messages, model: "gpt-4-vision-preview");
            var result = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
            {
                Assert.IsNotNull(partialResponse);
                Assert.NotNull(partialResponse.Choices);
                Assert.NotZero(partialResponse.Choices.Count);
            });
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishDetails}");
            result.GetUsage();
        }

        [Test]
        public async Task Test_12_GetChatVision_Texture()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ChatEndpoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, new List<Content>
                {
                    new Content(ContentType.Text, "What's in this image?"),
                    new Content(image)
                })
            };
            var chatRequest = new ChatRequest(messages, model: "gpt-4-vision-preview");
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Choices);
            Debug.Log($"{result.FirstChoice.Message.Role}: {result.FirstChoice.Message.Content} | Finish Reason: {result.FirstChoice.FinishDetails}");
            result.GetUsage();
        }

    }
}
