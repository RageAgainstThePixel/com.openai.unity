// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_05_Embeddings
    {
        [UnityTest]
        public IEnumerator Test_1_CreateEmbedding()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.EmbeddingsEndpoint);
                var result = await api.EmbeddingsEndpoint.CreateEmbeddingAsync("The food was delicious and the waiter...");
                Assert.IsNotNull(result);
                Assert.IsNotEmpty(result.Data);
            });
        }
    }
}
