// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Images;
using System;
using System.IO;
using System.Threading.Tasks;
using OpenAI.Models;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_05_Images
    {
        [Test]
        public async Task Test_1_GenerateImages()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var request = new ImageGenerationRequest("A house riding a velociraptor", Model.DallE_3);
            var results = await api.ImagesEndPoint.GenerateImageAsync(request);
            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_2_GenerateImages_B64_Json()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);

            var request = new ImageGenerationRequest("A house riding a velociraptor", Model.DallE_2, responseFormat: ResponseFormat.B64_Json);
            var results = await api.ImagesEndPoint.GenerateImageAsync(request);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }

        [Test]
        public async Task Test_3_CreateImageEdit_Path()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var results = await api.ImagesEndPoint.CreateImageEditAsync(Path.GetFullPath(imageAssetPath), Path.GetFullPath(maskAssetPath), "A sunlit indoor lounge area with a pool containing a flamingo", 1, ImageSize.Small);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_4_CreateImageEdit_Texture()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var results = await api.ImagesEndPoint.CreateImageEditAsync(image, mask, "A sunlit indoor lounge area with a pool containing a flamingo", 1, ImageSize.Small);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_5_CreateImageEdit_Texture_B64_Json()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var results = await api.ImagesEndPoint.CreateImageEditAsync(image, mask, "A sunlit indoor lounge area with a pool containing a flamingo", 1, ImageSize.Small, responseFormat: ResponseFormat.B64_Json);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_6_CreateImageEdit_MaskAsTransparency()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var maskAssetPath = AssetDatabase.GUIDToAssetPath("0be6be2fad590cc47930495d2ca37dd6");
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(maskAssetPath);
            var results = await api.ImagesEndPoint.CreateImageEditAsync(mask, null, "A sunlit indoor lounge area with a pool containing a flamingo", 1, ImageSize.Small);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_7_CreateImageVariation_Path()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var results = await api.ImagesEndPoint.CreateImageVariationAsync(Path.GetFullPath(imageAssetPath), 1, ImageSize.Small);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_8_CreateImageVariation_Texture()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var results = await api.ImagesEndPoint.CreateImageVariationAsync(image, 1, ImageSize.Small);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }

        [Test]
        public async Task Test_9_CreateImageVariation_Texture_B64_Json()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ImagesEndPoint);
            var imageAssetPath = AssetDatabase.GUIDToAssetPath("230fd778637d3d84d81355c8c13b1999");
            var image = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAssetPath);
            var results = await api.ImagesEndPoint.CreateImageVariationAsync(image, 1, ImageSize.Small, responseFormat: ResponseFormat.B64_Json);

            Assert.IsNotNull(results);
            Assert.NotZero(results.Count);

            foreach (var (path, texture) in results)
            {
                Debug.Log(path);
                Assert.IsNotNull(texture);
            }
        }
    }
}
