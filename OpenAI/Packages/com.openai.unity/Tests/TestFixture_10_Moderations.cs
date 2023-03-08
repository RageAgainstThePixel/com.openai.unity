// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_10_Moderations
    {
        [UnityTest]
        public IEnumerator Test_1_Moderate()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                var violationResponse = await api.ModerationsEndpoint.GetModerationAsync("I want to kill them.");
                Assert.IsTrue(violationResponse);

                var response = await api.ModerationsEndpoint.GetModerationAsync("I love you");
                Assert.IsFalse(response);
            });
        }
    }
}
