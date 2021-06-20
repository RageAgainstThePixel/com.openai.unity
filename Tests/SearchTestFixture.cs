// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class SearchTestFixture
    {
        private readonly string query = "Washington DC";
        private readonly string[] documents = { "Canada", "China", "USA", "Spain" };

        [UnityTest]
        public IEnumerator Test_1_GetSearchResults()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();

                Assert.IsNotNull(api.SearchEndpoint);

                var results = await api.SearchEndpoint.GetSearchResultsAsync(query, documents, Engine.Curie);

                Assert.IsNotNull(results);
                Assert.IsNotEmpty(results);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_GetBestMatch()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();

                Assert.IsNotNull(api.SearchEndpoint);

                var result = await api.SearchEndpoint.GetBestMatchAsync(query, documents, Engine.Curie);

                Assert.IsNotNull(result);
                Assert.AreEqual("USA", result);
            });
        }

        [UnityTest]
        public IEnumerator Test_3_GetBestMatchWithScore()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();

                Assert.IsNotNull(api.SearchEndpoint);

                var result = await api.SearchEndpoint.GetBestMatchWithScoreAsync(query, documents, Engine.Curie);

                Assert.IsNotNull(result);
                var (match, score) = result;
                Assert.AreEqual("USA", match);
                Assert.NotZero(score);
            });
        }
    }
}
