// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.Async;
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
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        [Function("Creates an image given a prompt.")]
        public async Task<IReadOnlyList<ImageResult>> GenerateImageAsync(ImageGenerationRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/generations"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageEditRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        [Function("Creates an edited or extended image given an original image and a prompt.")]
        public async Task<IReadOnlyList<ImageResult>> CreateImageEditAsync(ImageEditRequest request, CancellationToken cancellationToken = default)
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
            form.AddField("response_format", request.ResponseFormat.ToString().ToLower());

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
        /// <param name="request"><see cref="ImageVariationRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>A dictionary of file urls and the preloaded <see cref="Texture2D"/> that were downloaded.</returns>
        [Function("Creates a variation of a given image.")]
        public async Task<IReadOnlyList<ImageResult>> CreateImageVariationAsync(ImageVariationRequest request, CancellationToken cancellationToken = default)
        {
            var form = new WWWForm();
            using var imageData = new MemoryStream();
            await request.Image.CopyToAsync(imageData, cancellationToken);
            form.AddBinaryData("image", imageData.ToArray(), request.ImageName);
            form.AddField("n", request.Number.ToString());
            form.AddField("size", request.Size);
            form.AddField("response_format", request.ResponseFormat.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                form.AddField("user", request.User);
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/variations"), form, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            return await DeserializeResponseAsync(response, cancellationToken);
        }

        private async Task<IReadOnlyList<ImageResult>> DeserializeResponseAsync(Response response, CancellationToken cancellationToken = default)
        {
            response.Validate(EnableDebug);

            var imagesResponse = response.Deserialize<ImagesResponse>(client);

            if (imagesResponse?.Results is not { Count: not 0 })
            {
                throw new Exception($"No image content returned!\n{response.Body}");
            }

            await Rest.ValidateCacheDirectoryAsync();
            var downloads = imagesResponse.Results.Select(DownloadAsync).ToList();

            async Task DownloadAsync(ImageResult result)
            {
                await Awaiters.UnityMainThread;

                if (string.IsNullOrWhiteSpace(result.Url))
                {
                    var imageData = Convert.FromBase64String(result.B64_Json);
#if PLATFORM_WEBGL
                    result.Texture = new Texture2D(2, 2);
                    result.Texture.LoadImage(imageData);
#else
                    if (!Rest.TryGetDownloadCacheItem(result.B64_Json, out var localFilePath))
                    {
                        await File.WriteAllBytesAsync(localFilePath, imageData, cancellationToken).ConfigureAwait(true);
                    }

                    result.Texture = await Rest.DownloadTextureAsync(localFilePath, parameters: new RestParameters(debug: EnableDebug), cancellationToken: cancellationToken);

                    if (Rest.TryGetDownloadCacheItem(result.B64_Json, out var cachedPath))
                    {
                        result.CachedPath = cachedPath;
                    }
#endif
                }
                else
                {
                    result.Texture = await Rest.DownloadTextureAsync(result.Url, parameters: new RestParameters(debug: EnableDebug), cancellationToken: cancellationToken);

                    if (Rest.TryGetDownloadCacheItem(result.Url, out var cachedPath))
                    {
                        result.CachedPath = cachedPath;
                    }
                }
            }

            await Task.WhenAll(downloads).ConfigureAwait(true);
            return imagesResponse.Results;
        }
    }
}
