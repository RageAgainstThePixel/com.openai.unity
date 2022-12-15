// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Models;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_01_Models
    {
        [UnityTest]
        public IEnumerator Test_1_GetModels()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(Model.Davinci);
                Assert.IsNotNull(api.ModelsEndpoint);
                var results = await api.ModelsEndpoint.GetModelsAsync();
                Assert.IsNotNull(results);
                Assert.NotZero(results.Count);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_RetrieveModelDetails()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();
                Assert.IsNotNull(api.ModelsEndpoint);
                var models = await api.ModelsEndpoint.GetModelsAsync();
                Assert.IsNotEmpty(models);

                foreach (var model in models)
                {
                    Debug.Log(model.ToString());

                    try
                    {
                        var result = await api.ModelsEndpoint.GetModelDetailsAsync(model.Id);
                        Assert.IsNotNull(result);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Failed to retrieve engine data for {model.Id}\n{e}");
                    }
                }
            });
        }
    }
}
