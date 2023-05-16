// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Completions;
using OpenAI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_02_Completions
    {
        private const string CompletionPrompts = "One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight";

        [Test]
        public async Task Test_1_GetBasicCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.CompletionsEndpoint);
            var result = await api.CompletionsEndpoint.CreateCompletionAsync(
                CompletionPrompts,
                temperature: 0.1,
                maxTokens: 5,
                numOutputs: 5,
                model: Model.Davinci);
            Assert.IsNotNull(result);
            Assert.NotNull(result.Completions);
            Assert.NotZero(result.Completions.Count);
            Assert.That(result.Completions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
            Debug.Log(result);
        }

        [Test]
        public async Task Test_2_GetStreamingCompletion()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.CompletionsEndpoint);
            var allCompletions = new List<Choice>();

            await api.CompletionsEndpoint.StreamCompletionAsync(result =>
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Completions);
                Assert.NotZero(result.Completions.Count);
                allCompletions.AddRange(result.Completions);
            }, CompletionPrompts, temperature: 0.1, maxTokens: 5, numOutputs: 5);

            Assert.That(allCompletions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
            Debug.Log(allCompletions.FirstOrDefault());
        }

        [Test]
        public async Task Test_3_GetStreamingCompletionEnumerableAsync()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.CompletionsEndpoint);
            var allCompletions = new List<Choice>();

            await foreach (var result in api.CompletionsEndpoint.StreamCompletionEnumerableAsync(
                               CompletionPrompts,
                               temperature: 0.1,
                               maxTokens: 5,
                               numOutputs: 5,
                               model: Model.Davinci))
            {
                Assert.IsNotNull(result);
                Assert.NotNull(result.Completions);
                Assert.NotZero(result.Completions.Count);
                allCompletions.AddRange(result.Completions);
            }

            Assert.That(allCompletions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
            Debug.Log(allCompletions.FirstOrDefault());
        }
    }
}
