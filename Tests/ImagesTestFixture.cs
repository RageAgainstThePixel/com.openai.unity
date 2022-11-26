// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Images;
using System.Collections;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class ImagesTestFixture
    {
        [UnityTest]
        public IEnumerator Test_1_GenerateImage()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient();
                var results = await api.ImageGenerationEndPoint.GenerateImageAsync("A house riding a velociraptor", 1, ImageSize.Small);

                Assert.IsNotNull(results);
                Assert.NotZero(results.Count);
            });
        }
    }
}
