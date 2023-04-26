// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Images
{
    /// <summary>
    /// Given a prompt and/or an input image, the model will generate a new image.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/images"/>
    /// </summary>
    public sealed class ImagesEndpoint : BaseEndPoint
    {
        /// <inheritdoc />
        internal ImagesEndpoint(OpenAIClient api) : base(api) { }

        /// <inheritdoc />
        protected override string Root => "images";

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="prompt">
        /// A text description of the desired image(s). The maximum length is 1000 characters.
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
        /// <param name="responseFormat">
        /// The format in which the generated images are returned. Must be one of url or b64_json.
        /// <para/> Defaults to <see cref="ResponseFormat.Url"/>
        /// </param>
        /// <param name="cancellationToken">
        /// Optional, <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> GenerateImageAsync(
            string prompt,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ResponseFormat responseFormat = ResponseFormat.Url,
            CancellationToken cancellationToken = default)
            => await GenerateImageAsync(new ImageGenerationRequest(prompt, numberOfResults, size, user, responseFormat), cancellationToken);

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default)
        {
            var jsonContent = JsonConvert.SerializeObject(request, Api.JsonSerializationOptions).ToJsonStringContent();
            var response = await Api.Client.PostAsync(GetUrl("/generations"), jsonContent, cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
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
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <param name="responseFormat">
        /// The format in which the generated images are returned. Must be one of url or b64_json.
        /// <para/> Defaults to <see cref="ResponseFormat.Url"/>
        /// </param>
        /// <param name="cancellationToken">
        /// Optional, <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.
        /// </returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(
            string image,
            string mask,
            string prompt,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ResponseFormat responseFormat = ResponseFormat.Url,
            CancellationToken cancellationToken = default)
            => await CreateImageEditAsync(new ImageEditRequest(image, mask, prompt, numberOfResults, size, user, responseFormat), cancellationToken);

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
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <param name="responseFormat">
        /// The format in which the generated images are returned. Must be one of url or b64_json.
        /// <para/> Defaults to <see cref="ResponseFormat.Url"/>
        /// </param>
        /// <param name="cancellationToken">
        /// Optional, <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.
        /// </returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(
            Texture2D image,
            Texture2D mask,
            string prompt,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ResponseFormat responseFormat = ResponseFormat.Url,
            CancellationToken cancellationToken = default)
            => await CreateImageEditAsync(new ImageEditRequest(image, mask, prompt, numberOfResults, size, user, responseFormat), cancellationToken);

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageEditRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(ImageEditRequest request, CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData, cancellationToken);
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", request.ImageName);

            if (request.Mask != null)
            {
                using var maskData = new MemoryStream();
                await request.Mask.CopyToAsync(maskData, cancellationToken);
                content.Add(new ByteArrayContent(maskData.ToArray()), "mask", request.MaskName);
            }

            content.Add(new StringContent(request.Prompt), "prompt");
            content.Add(new StringContent(request.Number.ToString()), "n");
            content.Add(new StringContent(request.Size), "size");
            content.Add(new StringContent(request.ResponseFormat), "response_format");

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                content.Add(new StringContent(request.User), "user");
            }

            request.Dispose();

            var response = await Api.Client.PostAsync(GetUrl("/edits"), content, cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="imagePath">
        /// The image to edit. Must be a valid PNG file, less than 4MB, and square.
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
        /// <param name="responseFormat">
        /// The format in which the generated images are returned. Must be one of url or b64_json.
        /// <para/> Defaults to <see cref="ResponseFormat.Url"/>
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(
            string imagePath,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ResponseFormat responseFormat = ResponseFormat.Url,
            CancellationToken cancellationToken = default)
            => await CreateImageVariationAsync(new ImageVariationRequest(imagePath, numberOfResults, size, user, responseFormat), cancellationToken);

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="texture">
        /// The texture to edit. Must be a valid PNG file, less than 4MB, and square. Read/Write should be enabled and Compression set to None.
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
        /// <param name="responseFormat">
        /// The format in which the generated images are returned. Must be one of url or b64_json.
        /// <para/> Defaults to <see cref="ResponseFormat.Url"/>
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(
            Texture2D texture,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ResponseFormat responseFormat = ResponseFormat.Url,
            CancellationToken cancellationToken = default)
            => await CreateImageVariationAsync(new ImageVariationRequest(texture, numberOfResults, size, user, responseFormat), cancellationToken);

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="request"><see cref="ImageVariationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(ImageVariationRequest request, CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData, cancellationToken);
            content.Add(new ByteArrayContent(imageData.ToArray()), "image", request.ImageName);
            content.Add(new StringContent(request.Number.ToString()), "n");
            content.Add(new StringContent(request.Size), "size");
            content.Add(new StringContent(request.ResponseFormat), "response_format");

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                content.Add(new StringContent(request.User), "user");
            }

            request.Dispose();

            var response = await Api.Client.PostAsync(GetUrl("/variations"), content, cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
        }

        private async Task<IReadOnlyDictionary<string, Texture2D>> DeserializeResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var resultAsString = await response.ReadAsStringAsync();
            var imagesResponse = JsonConvert.DeserializeObject<ImagesResponse>(resultAsString, Api.JsonSerializationOptions);

            if (imagesResponse?.Data == null || imagesResponse.Data.Count == 0)
            {
                throw new HttpRequestException($"{nameof(DeserializeResponseAsync)} returned no results!  HTTP status code: {response.StatusCode}. Response body: {resultAsString}");
            }

            imagesResponse.SetResponseData(response.Headers);

            var images = new ConcurrentDictionary<string, Texture2D>();
            var downloads = imagesResponse.Data.Select(DownloadAsync).ToList();

            await Task.WhenAll(downloads);

            async Task DownloadAsync(ImageResult result)
            {
                string resultString;
                string localFilePath;

                if (string.IsNullOrWhiteSpace(result.Url))
                {
                    resultString = result.B64_Json;
                    var imageData = Convert.FromBase64String(resultString);
                    await Rest.ValidateCacheDirectoryAsync();

                    if (!Rest.TryGetDownloadCacheItem(resultString, out localFilePath))
                    {
                        await using FileStream fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.ReadWrite);
                        await fileStream.WriteAsync(imageData, cancellationToken);
                    }

                    resultString = $"file://{localFilePath}";
                }
                else
                {
                    resultString = result.Url;
                }

                var texture = await Rest.DownloadTextureAsync(resultString, cancellationToken: cancellationToken);

                if (Rest.TryGetDownloadCacheItem(resultString, out localFilePath))
                {
                    images.TryAdd(localFilePath, texture);
                }
                else
                {
                    Debug.LogError($"Failed to find cached item for {resultString}");
                }
            }

            return images;
        }
    }
}
