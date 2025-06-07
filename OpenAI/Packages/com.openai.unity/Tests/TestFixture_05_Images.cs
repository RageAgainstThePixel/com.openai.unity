// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Images;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_05_Images : AbstractTestFixture
    {
        private string testDirectory;

        [OneTimeSetUp]
        public void Setup()
        {
            testDirectory = Path.GetFullPath($"{Application.dataPath}/Assets/Tests/{nameof(TestFixture_05_Images)}");

            if (!Directory.Exists(testDirectory))
            {
                Directory.CreateDirectory(testDirectory);
            }
        }

        [Test]
        public async Task Test_01_01_GenerateImages()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var request = new ImageGenerationRequest("A house riding a velociraptor", outputFormat: "jpeg");
            var imageResults = await OpenAIClient.ImagesEndPoint.GenerateImageAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var image in imageResults)
            {
                Assert.IsNotNull(image);
                Assert.IsFalse(string.IsNullOrWhiteSpace(image.B64_Json));
                var imageBytes = Convert.FromBase64String(image.B64_Json);
                Assert.IsNotNull(imageBytes);
                var path = Path.Combine(testDirectory, $"{nameof(Test_01_01_GenerateImages)}-{DateTime.UtcNow:yyyyMMddHHmmss}.jpeg");
                await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
            }
        }

        [Test]
        public async Task Test_02_01_CreateImageEdit_Path()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var request = new ImageEditRequest(
                prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                imagePath: Path.GetFullPath(imageAssetPath),
                maskPath: Path.GetFullPath(maskAssetPath));
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var image in imageResults)
            {
                Assert.IsNotNull(image.Texture);
                Debug.Log(image.ToString());
            }
        }

        [Test]
        public async Task Test_02_03_CreateImageEdit_Texture()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var request = new ImageEditRequest(
                prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                texture: image,
                mask: mask);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Assert.IsNotNull(result.Texture);
                Debug.Log(result.ToString());
            }
        }

        [Test]
        public async Task Test_02_05_CreateImageEdit_MaskAsTransparency()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var request = new ImageEditRequest(
                prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                texture: mask);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var image in imageResults)
            {
                Assert.IsNotNull(image.Texture);
                Debug.Log(image.ToString());
            }
        }

        [Test]
        public async Task Test_03_01_CreateImageVariation_Path()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var request = new ImageVariationRequest(Path.GetFullPath(imageAssetPath), size: ImageSize.Small);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var image in imageResults)
            {
                Assert.IsNotNull(image.Texture);
                Debug.Log(image.ToString());
            }
        }

        [Test]
        public async Task Test_03_02_CreateImageVariation_Texture()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var request = new ImageVariationRequest(image, size: "256x256");
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }

        [Test]
        public async Task Test_03_04_CreateImageVariation_Texture_B64_Json()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var request = new ImageVariationRequest(image, size: "256x256", responseFormat: ImageResponseFormat.B64_Json);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Assert.IsNotNull(result.Texture);
                Debug.Log(result.ToString());
            }
        }
    }
}
