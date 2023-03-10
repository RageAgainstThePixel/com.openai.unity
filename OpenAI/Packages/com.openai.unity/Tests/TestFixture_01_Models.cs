// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_01_Models
    {
        [Test]
        public async Task Test_1_GetModels()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            Assert.IsNotNull(api.ModelsEndpoint);
            var results = await api.ModelsEndpoint.GetModelsAsync();
            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);
        }

        [Test]
        public async Task Test_2_RetrieveModelDetails()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
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
                    Debug.LogWarning($"No Model details found for {model.Id}\n{e}");
                }
            }
        }
    }
}
