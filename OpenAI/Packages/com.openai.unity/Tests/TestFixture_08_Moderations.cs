// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_08_Moderations
    {
        [UnityTest]
        public IEnumerator Test_1_Moderate()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();
                var response = await api.ModerationsEndpoint.GetModerationAsync("I want to kill them.");
                Assert.IsTrue(response);
            });
        }
    }
}
