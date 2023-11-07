// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed class ImagesEndpoint : OpenAIBaseEndpoint
    {
        /// <inheritdoc />
        internal ImagesEndpoint(OpenAIClient client) : base(client) { }

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
        public async Task<IReadOnlyDictionary<string, Texture2D>> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/generations"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
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
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageEditAsync(ImageEditRequest request, CancellationToken cancellationToken = default)
        {
            var form = new WWWForm();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData, cancellationToken);
            form.AddBinaryData("image", imageData.ToArray(), request.ImageName);

            if (request.Mask != null)
            {
                using var maskData = new MemoryStream();
                await request.Mask.CopyToAsync(maskData, cancellationToken);
                form.AddBinaryData("mask", maskData.ToArray(), request.MaskName);
            }

            form.AddField("prompt", request.Prompt);
            form.AddField("n", request.Number.ToString());
            form.AddField("size", request.Size);
            form.AddField("response_format", request.ResponseFormat);

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                form.AddField("user", request.User);
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/edits"), form, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
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
        public async Task<IReadOnlyDictionary<string, Texture2D>> CreateImageVariationAsync(ImageVariationRequest request, CancellationToken cancellationToken = default)
        {
            var form = new WWWForm();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData, cancellationToken);
            form.AddBinaryData("image", imageData.ToArray(), request.ImageName);
            form.AddField("n", request.Number.ToString());
            form.AddField("size", request.Size);
            form.AddField("response_format", request.ResponseFormat);

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                form.AddField("user", request.User);
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/variations"), form, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
        }

        private async Task<IReadOnlyDictionary<string, Texture2D>> DeserializeResponseAsync(Response response, CancellationToken cancellationToken = default)
        {
            response.Validate(EnableDebug);

            var imagesResponse = response.DeserializeResponse<ImagesResponse>(response.Body);

            if (imagesResponse?.Data == null || imagesResponse.Data.Count == 0)
            {
                throw new Exception($"No image content returned!\n{response.Body}");
            }

            var images = new ConcurrentDictionary<string, Texture2D>();
            var downloads = imagesResponse.Data.Select(DownloadAsync).ToList();

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
                        await File.WriteAllBytesAsync(localFilePath, imageData, cancellationToken);
                    }

                    resultString = $"file://{localFilePath}";
                }
                else
                {
                    resultString = result.Url;
                }

                var texture = await Rest.DownloadTextureAsync(resultString, parameters: null, cancellationToken: cancellationToken);

                if (Rest.TryGetDownloadCacheItem(resultString, out localFilePath))
                {
                    images.TryAdd(localFilePath, texture);
                }
                else
                {
                    Debug.LogError($"Failed to find cached item for {resultString}");
                }
            }

            await Task.WhenAll(downloads).ConfigureAwait(true);
            return images;
        }
    }
}
