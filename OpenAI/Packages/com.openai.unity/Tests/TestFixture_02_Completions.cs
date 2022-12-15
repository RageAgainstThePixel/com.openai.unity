// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Completions;
using OpenAI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_02_Completions
    {
        private readonly string completionPrompts = "One Two Three Four Five Six Seven Eight Nine One Two Three Four Five Six Seven Eight";

        [UnityTest]
        public IEnumerator Test_1_GetBasicCompletion()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();
                Assert.IsNotNull(api.CompletionEndpoint);
                var result = await api.CompletionEndpoint.CreateCompletionAsync(
                    completionPrompts,
                    temperature: 0.1,
                    max_tokens: 5,
                    numOutputs: 5,
                    model: Model.Davinci);
                Assert.IsNotNull(result);
                Assert.NotNull(result.Completions);
                Assert.NotZero(result.Completions.Count);
                Assert.That(result.Completions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
                Debug.Log(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_GetStreamingCompletion()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();
                Assert.IsNotNull(api.CompletionEndpoint);
                var allCompletions = new List<Choice>();

                await api.CompletionEndpoint.StreamCompletionAsync(result =>
                {
                    Assert.IsNotNull(result);
                    Assert.NotNull(result.Completions);
                    Assert.NotZero(result.Completions.Count);
                    allCompletions.AddRange(result.Completions);

                    foreach (var choice in result.Completions)
                    {
                        Debug.Log(choice);
                    }
                }, completionPrompts, temperature: 0.1, max_tokens: 5, numOutputs: 5);

                Assert.That(allCompletions.Any(c => c.Text.Trim().ToLower().StartsWith("nine")));
            });
        }
    }
}
