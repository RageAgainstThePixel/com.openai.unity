// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Threading.Tasks;

namespace OpenAI.Tests
{
    internal class TestFixture_06_Embeddings
    {
        [Test]
        public async Task Test_1_CreateEmbedding()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.EmbeddingsEndpoint);
            var embedding = await api.EmbeddingsEndpoint.CreateEmbeddingAsync("The food was delicious and the waiter...");
            Assert.IsNotNull(embedding);
            Assert.IsNotEmpty(embedding.Data);
        }

        [Test]
        public async Task Test_2_CreateEmbeddingsWithMultipleInputs()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.EmbeddingsEndpoint);
            var embeddings = new[]
            {
                "The food was delicious and the waiter...",
                "The food was terrible and the waiter..."
            };
            var embedding = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(embeddings);
            Assert.IsNotNull(embedding);
            Assert.AreEqual(embedding.Data.Count, 2);
        }
    }
}
