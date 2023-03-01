// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_03_Chat
    {
        [UnityTest]
        public IEnumerator Test_1_GetChatCompletion()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.ChatEndpoint);
                var chatPrompts = new List<ChatPrompt>();
                var chatRequest = new ChatRequest(chatPrompts);
                var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);
                Debug.Log(result);
            });
        }
    }
}
