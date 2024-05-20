// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Images;
using OpenAI.Models;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_05_Images : AbstractTestFixture
    {
        [Test]
        public async Task Test_01_01_GenerateImages()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var request = new ImageGenerationRequest("A house riding a velociraptor", Model.DallE_3);
            var imageResults = await OpenAIClient.ImagesEndPoint.GenerateImageAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }

        [Test]
        public async Task Test_01_02_GenerateImages_B64_Json()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);

            var request = new ImageGenerationRequest("A house riding a velociraptor", Model.DallE_2, responseFormat: ImageResponseFormat.B64_Json);
            var imageResults = await OpenAIClient.ImagesEndPoint.GenerateImageAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var image in imageResults)
            {
                Assert.IsNotNull(image);
                Debug.Log(image);
            }
        }

        [Test]
        public async Task Test_02_01_CreateImageEdit_Path()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var request = new ImageEditRequest(Path.GetFullPath(imageAssetPath), Path.GetFullPath(maskAssetPath), "A sunlit indoor lounge area with a pool containing a flamingo", size: ImageSize.Small);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
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
            var request = new ImageEditRequest(image, mask, "A sunlit indoor lounge area with a pool containing a flamingo", size: ImageSize.Small);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }

        [Test]
        public async Task Test_02_04_CreateImageEdit_Texture_B64_Json()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var request = new ImageEditRequest(image, mask, "A sunlit indoor lounge area with a pool containing a flamingo", size: ImageSize.Small, responseFormat: ImageResponseFormat.B64_Json);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }

        [Test]
        public async Task Test_02_05_CreateImageEdit_MaskAsTransparency()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var request = new ImageEditRequest(mask, null, "A sunlit indoor lounge area with a pool containing a flamingo", size: ImageSize.Small);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
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

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }

        [Test]
        public async Task Test_03_02_CreateImageVariation_Texture()
        {
            Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var request = new ImageVariationRequest(image, size: ImageSize.Small);
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
            var request = new ImageVariationRequest(image, size: ImageSize.Small, responseFormat: ImageResponseFormat.B64_Json);
            var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

            Assert.IsNotNull(imageResults);
            Assert.NotZero(imageResults.Count);

            foreach (var result in imageResults)
            {
                Debug.Log(result.ToString());
                Assert.IsNotNull(result.Texture);
            }
        }
    }
}
