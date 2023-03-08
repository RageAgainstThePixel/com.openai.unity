// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Edits;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_04_Edits
    {
        [UnityTest]
        public IEnumerator Test_1_GetBasicEdit()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.EditsEndpoint);
                var request = new EditRequest("What day of the wek is it?", "Fix the spelling mistakes");
                var result = await api.EditsEndpoint.CreateEditAsync(request);
                Assert.IsNotNull(result);
                Assert.NotNull(result.Choices);
                Assert.NotZero(result.Choices.Count);
                Assert.IsTrue(result.ToString().Contains("week"));
                Debug.Log(result);
            });
        }
    }
}
