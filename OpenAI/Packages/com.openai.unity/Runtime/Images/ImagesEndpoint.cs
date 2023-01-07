// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Images
{
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    public sealed class ImagesEndpoint : BaseEndPoint
    {
        /// <inheritdoc />
        internal ImagesEndpoint(OpenAIClient api) : base(api) { }

        /// <inheritdoc />
        protected override string GetEndpoint() => $"{Api.BaseUrl}images/";

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
        /// <param name="numberOfResults">The number of images to generate. Must be between 1 and 10.</param>
        /// <param name="size">The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.</param>
        /// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
        /// <returns>An array of generated textures.</returns>
        public async Task<IReadOnlyDictionary<string, Texture2D>> GenerateImageAsync(string prompt, int numberOfResults = 1, ImageSize size = ImageSize.Large, string user = null)
            => await GenerateImageAsync(new ImageGenerationRequest(prompt, numberOfResults, size, user));

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <returns>An array of generated textures.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> GenerateImageAsync(ImageGenerationRequest request)
        {
            var jsonContent = JsonConvert.SerializeObject(request, Api.JsonSerializationOptions);
            var response = await Api.Client.PostAsync($"{GetEndpoint()}generations", jsonContent.ToJsonStringContent()).ConfigureAwait(true);

            return response.IsSuccessStatusCode
                ? await DeserializeResponseAsync(response).ConfigureAwait(true)
                : throw new HttpRequestException(
                    $"{nameof(GenerateImageAsync)} Failed!  HTTP status code: {response.StatusCode}. Request body: {jsonContent}");
        }

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="image">
        /// The image to edit. Must be a valid PNG file, less than 4MB, and square.
        /// If mask is not provided, image must have transparency, which will be used as the mask.
        /// </param>
        /// <param name="mask">
        /// An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where image should be edited.
        /// Must be a valid PNG file, less than 4MB, and have the same dimensions as image.
        /// </param>
        /// <param name="prompt">
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </param>
        /// <param name="numberOfResults">
        /// The number of images to generate. Must be between 1 and 10.
        /// </param>
        /// <param name="size">
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.
        /// </param>
        /// <param name="user">A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.</param>
        /// <returns>An array of generated textures.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(string image, string mask, string prompt, int numberOfResults = 1, ImageSize size = ImageSize.Large, string user = null)
            => await CreateImageEditAsync(new ImageEditRequest(image, mask, prompt, numberOfResults, size, user));

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageEditRequest"/></param>
        /// <returns>An array of generated textures.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(ImageEditRequest request)
        {
            using var content = new MultipartFormDataContent();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData).ConfigureAwait(true);
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", request.ImageName);
            using var maskData = new MemoryStream();
            await request.Mask.CopyToAsync(maskData).ConfigureAwait(true);
            content.Add(new ByteArrayContent(maskData.ToArray()), "mask", request.MaskName);
            content.Add(new StringContent(request.Prompt), "prompt");
            content.Add(new StringContent(request.Number.ToString()), "n");
            content.Add(new StringContent(request.Size), "size");

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                content.Add(new StringContent(request.User), "user");
            }

            var response = await Api.Client.PostAsync($"{GetEndpoint()}edits", content).ConfigureAwait(true);
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            return response.IsSuccessStatusCode
                ? await DeserializeResponseAsync(response).ConfigureAwait(true)
                : throw new HttpRequestException(
                    $"{nameof(CreateImageEditAsync)} Failed!  HTTP status code: {response.StatusCode}. Response: {responseAsString}");
        }

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="imagePath">
        /// The image to edit. Must be a valid PNG file, less than 4MB, and square.
        /// If mask is not provided, image must have transparency, which will be used as the mask.
        /// </param>
        /// <param name="numberOfResults">
        /// The number of images to generate. Must be between 1 and 10.
        /// </param>
        /// <param name="size">
        /// The size of the generated images. Must be one of 256x256, 512x512, or 1024x1024.
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <returns></returns>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(string imagePath, int numberOfResults = 1, ImageSize size = ImageSize.Large, string user = null)
            => await CreateImageVariationAsync(new ImageVariationRequest(imagePath, numberOfResults, size, user));

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="request"><see cref="ImageVariationRequest"/></param>
        /// <returns>An array of generated textures.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(ImageVariationRequest request)
        {
            using var content = new MultipartFormDataContent();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData).ConfigureAwait(true);
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", request.ImageName);
            content.Add(new StringContent(request.Number.ToString()), "n");
            content.Add(new StringContent(request.Size), "size");

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                content.Add(new StringContent(request.User), "user");
            }

            var response = await Api.Client.PostAsync($"{GetEndpoint()}variations", content).ConfigureAwait(true);
            var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            return response.IsSuccessStatusCode
                ? await DeserializeResponseAsync(response).ConfigureAwait(true)
                : throw new HttpRequestException(
                    $"{nameof(CreateImageVariationAsync)} Failed!  HTTP status code: {response.StatusCode}. Response: {responseAsString}");
        }

        private async Task<IReadOnlyDictionary<string, Texture2D>> DeserializeResponseAsync(HttpResponseMessage response)
        {
            Debug.Assert(response.IsSuccessStatusCode);
            var resultAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var imagesResponse = JsonConvert.DeserializeObject<ImagesResponse>(resultAsString, Api.JsonSerializationOptions);

            if (imagesResponse?.Data == null || imagesResponse.Data.Count == 0)
            {
                throw new HttpRequestException($"{nameof(DeserializeResponseAsync)} returned no results!  HTTP status code: {response.StatusCode}. Response body: {resultAsString}");
            }

            imagesResponse.SetResponseData(response.Headers);

            var images = new ConcurrentDictionary<string, Texture2D>();
            var downloads = imagesResponse.Data.Select(DownloadAsync).ToList();

            await Task.WhenAll(downloads).ConfigureAwait(true);

            async Task DownloadAsync(ImageResult result)
            {
                var texture = await Rest.DownloadTextureAsync(result.Url).ConfigureAwait(true);

                if (Rest.TryGetDownloadCacheItem(result.Url, out var localFilePath))
                {
                    images.TryAdd(localFilePath, texture);
                }
                else
                {
                    Debug.LogError($"Failed to find cached item for {result.Url}");
                }
            }

            return images;
        }
    }
}
