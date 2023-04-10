// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
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
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            Assert.IsNotNull(result);
            Assert.NotNull(result.Choices);
            Assert.NotZero(result.Choices.Count);
            Debug.Log(result.FirstChoice);
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
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var allContent = new List<string>();

            await api.ChatEndpoint.StreamCompletionAsync(chatRequest, result =>
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);
                allContent.Add(result.FirstChoice);
            });

            Debug.Log(string.Join(string.Empty, allContent));
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
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var allContent = new List<string>();

            await foreach (var result in api.ChatEndpoint.StreamCompletionEnumerableAsync(chatRequest))
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);
                allContent.Add(result.FirstChoice);
            }

            Debug.Log(string.Join(string.Empty, allContent));
        }
    }
}
