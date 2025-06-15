// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Images;
using OpenAI.Models;
using System;
using System.Collections.Generic;
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
            testDirectory = Path.GetFullPath($"{Application.dataPath}/Tests/{nameof(TestFixture_05_Images)}");

            if (!Directory.Exists(testDirectory))
            {
                Directory.CreateDirectory(testDirectory);
            }
        }

        [Test]
        public async Task Test_01_01_GenerateImages()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var request = new ImageGenerationRequest("A house riding a velociraptor", outputFormat: "jpeg");
                var imageResults = await OpenAIClient.ImagesEndPoint.GenerateImageAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_01_01_GenerateImages)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.jpeg");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_02_01_CreateImageEdit_Path()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
                var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
                var request = new ImageEditRequest(
                    prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                    imagePath: Path.GetFullPath(imageAssetPath),
                    maskPath: Path.GetFullPath(maskAssetPath),
                    model: Model.GPT_Image_1);
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_02_01_CreateImageEdit_Path)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_02_02_CreateImageEdit_Texture()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
                var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
                var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
                var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
                var request = new ImageEditRequest(
                    prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                    texture: image,
                    mask: mask,
                    model: Model.GPT_Image_1);
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_02_02_CreateImageEdit_Texture)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_02_03_CreateImageEdit_MaskAsTransparency()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
                var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
                var request = new ImageEditRequest(
                    prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                    texture: mask,
                    model: Model.GPT_Image_1);
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_02_03_CreateImageEdit_MaskAsTransparency)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_02_04_CreateImageEdit_MultipleFiles()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
                var images = new Dictionary<string, Stream>();

                for (var i = 0; i < 16; i++)
                {
                    images.Add($"image_{i}.png", File.OpenRead(Path.GetFullPath(imageAssetPath)));
                }

                var request = new ImageEditRequest(
                    prompt: "A sunlit indoor lounge area with a pool containing a flamingo",
                    images: images,
                    model: Model.GPT_Image_1);
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageEditAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_02_04_CreateImageEdit_MultipleFiles)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_03_01_CreateImageVariation_Path()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
                var request = new ImageVariationRequest(Path.GetFullPath(imageAssetPath), size: "256x256");
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                foreach (var result in imageResults)
                {
                    Assert.IsNotNull(result.Texture);
                    Debug.Log(result.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_03_02_CreateImageVariation_Texture()
        {
            try
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
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [Test]
        public async Task Test_03_03_CreateImageVariation_Texture_B64_Json()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.ImagesEndPoint);
                var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
                var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
                var request = new ImageVariationRequest(image, size: "256x256", responseFormat: ImageResponseFormat.B64_Json);
                var imageResults = await OpenAIClient.ImagesEndPoint.CreateImageVariationAsync(request);

                Assert.IsNotNull(imageResults);
                Assert.NotZero(imageResults.Count);

                for (var i = 0; i < imageResults.Count; i++)
                {
                    var result = imageResults[i];
                    Assert.IsNotNull(result);
                    Assert.IsNotNull(result.Texture);
                    Assert.IsFalse(string.IsNullOrWhiteSpace(result.B64_Json));
                    var imageBytes = Convert.FromBase64String(result.B64_Json);
                    Assert.IsNotNull(imageBytes);
                    var path = Path.Combine(testDirectory, $"{nameof(Test_03_03_CreateImageVariation_Texture_B64_Json)}-{i}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");
                    await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }
    }
}
