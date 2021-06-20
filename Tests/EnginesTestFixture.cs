// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class EnginesTestFixture
    {
        [UnityTest]
        public IEnumerator Test_1_GetEngines()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(Engine.Davinci);

                var results = await api.EnginesEndpoint.GetEnginesAsync();

                Assert.IsNotNull(results);
                Assert.NotZero(results.Count);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_RetrieveEngineDetails()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(Engine.Davinci);

                var engines = await api.EnginesEndpoint.GetEnginesAsync();
                Assert.IsNotEmpty(engines);

                foreach (var engine in engines)
                {
                    var result = await api.EnginesEndpoint.GetEngineDetailsAsync(engine.EngineName);
                    Assert.IsNotNull(result);
                }
            });
        }
    }
}
