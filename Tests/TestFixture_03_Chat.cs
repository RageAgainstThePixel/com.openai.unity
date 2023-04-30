// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Chat;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_03_Chat
    {
        [Test]
        public async Task Test_1_GetChatCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, "Who won the world series in 2020?"),
                new Message(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new Message(Role.User, "Where was it played?"),
            };
            var choiceCount = 2;
            var chatRequest = new ChatRequest(messages, number: choiceCount);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.NotNull(result.Choices);
            Assert.NotZero(result.Choices.Count);
            Assert.IsTrue(result.Choices.Count == choiceCount);

            foreach (var choice in result.Choices)
            {
                Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content}");
            }
        }

        [Test]
        public async Task Test_2_GetChatStreamingCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, "Who won the world series in 2020?"),
                new Message(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new Message(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages);
            var finalResult = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, result =>
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);

                foreach (var choice in result.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    Debug.Log($"[{choice.Index}] {choice.Delta.Content}");
                }

                foreach (var choice in result.Choices.Where(choice => choice.Message?.Content != null))
                {
                    Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content}");
                }
            });

            Assert.IsNotNull(finalResult);
        }

        [Test]
        public async Task Test_3_GetChatStreamingCompletionEnumerableAsync()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            Assert.IsNotNull(api.ChatEndpoint);
            var messages = new List<Message>
            {
                new Message(Role.System, "You are a helpful assistant."),
                new Message(Role.User, "Who won the world series in 2020?"),
                new Message(Role.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                new Message(Role.User, "Where was it played?"),
            };
            var chatRequest = new ChatRequest(messages);
            await foreach (var result in api.ChatEndpoint.StreamCompletionEnumerableAsync(chatRequest))
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);

                foreach (var choice in result.Choices.Where(choice => choice.Delta?.Content != null))
                {
                    Debug.Log($"[{choice.Index}] {choice.Delta.Content}");
                }

                foreach (var choice in result.Choices.Where(choice => choice.Message?.Content != null))
                {
                    Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message.Content}");
                }
            }
        }
    }
}
