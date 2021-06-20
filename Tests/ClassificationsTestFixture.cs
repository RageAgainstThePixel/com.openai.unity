// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class ClassificationsTestFixture
    {
        private readonly string query = "It is a raining day :(";
        private readonly string[] labels = { "Positive", "Negative", "Neutral" };
        private readonly Dictionary<string, string> examples = new Dictionary<string, string>
        {
            { "A happy moment", "Positive" },
            { "I am sad.", "Negative" },
            { "I am feeling awesome", "Positive" }
        };

        [UnityTest]
        public IEnumerator Test_1_GetClassificationResults()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();

                Assert.IsNotNull(api.ClassificationEndpoint);

                var result = await api.ClassificationEndpoint.GetClassificationAsync(new ClassificationRequest(query, examples, labels));

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Label == "Negative");
            });
        }
    }
}
