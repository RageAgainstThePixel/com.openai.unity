// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using Utilities.Extensions;
using OpenAI.Images;
using Utilities.Async;
using System;
using Utilities.WebRequestRest;

#if !PLATFORM_WEBGL
using System.IO;
#endif

namespace OpenAI.Extensions
{
    public static class TextureExtensions
    {
        internal static async Task<(Texture2D, Uri)> ConvertFromBase64Async(string b64, bool debug, CancellationToken cancellationToken)
        {
            using var imageData = NativeArrayExtensions.FromBase64String(b64, Allocator.Persistent);
#if PLATFORM_WEBGL
            var texture = new Texture2D(2, 2);
#if UNITY_6000_0_OR_NEWER
            texture.LoadImage(imageData);
#else
            texture.LoadImage(imageData.ToArray());
#endif // UNITY_6000_0_OR_NEWER
            return await Task.FromResult((texture, null as Uri));
#else
            if (!Rest.TryGetDownloadCacheItem(b64, out var localFilePath))
            {
                await using var fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write);
                await fs.WriteAsync(imageData, cancellationToken: cancellationToken);
                localFilePath = $"file://{localFilePath}";
            }

            var texture = await Rest.DownloadTextureAsync(localFilePath, parameters: new RestParameters(debug: debug), cancellationToken: cancellationToken);
            Rest.TryGetDownloadCacheItem(b64, out var cachedPath);
            Uri cachedUri = null;

            if (!string.IsNullOrWhiteSpace(cachedPath))
            {
                cachedUri = new Uri(cachedPath);
            }

            return (texture, cachedUri);
#endif // !PLATFORM_WEBGL
        }

        /// <summary>
        /// Loads a Texture2D from an ImageResult, handling base64, cached path, or URL.
        /// </summary>
        /// <param name="imageResult">The <see cref="ImageResult"/> to load the texture for.</param>
        /// <param name="debug">Optional, debug flag.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// A tuple containing the converted <see cref="Texture2D"/> and the cached file path as a <see cref="Uri"/>.
        /// </returns>
        public static async Task<(Texture2D, Uri)> LoadTextureAsync(this ImageResult imageResult, bool debug = false, CancellationToken cancellationToken = default)
        {
            await Awaiters.UnityMainThread;

            if (imageResult.Texture.IsNull())
            {
                if (!string.IsNullOrWhiteSpace(imageResult.B64_Json))
                {
                    var (texture, cachedUri) = await ConvertFromBase64Async(imageResult.B64_Json, debug, cancellationToken);
                    imageResult.Texture = texture;
                    imageResult.CachedPathUri = cachedUri;
                }
                else
                {
                    Texture2D texture;
                    Uri cachedPath;

                    if (imageResult.CachedPathUri != null)
                    {
                        texture = await Rest.DownloadTextureAsync(imageResult.CachedPathUri, parameters: new RestParameters(debug: debug), cancellationToken: cancellationToken);
                        cachedPath = imageResult.CachedPathUri;
                    }
                    else if (imageResult.Uri != null)
                    {
                        texture = await Rest.DownloadTextureAsync(imageResult.Uri, parameters: new RestParameters(debug: debug), cancellationToken: cancellationToken);
                        cachedPath = Rest.TryGetDownloadCacheItem(imageResult.Uri, out var path) ? path : null;
                    }
                    else
                    {
                        throw new InvalidOperationException("ImageResult does not contain valid image data.");
                    }

                    imageResult.Texture = texture;
                    imageResult.CachedPathUri = cachedPath;
                }
            }

            return (imageResult.Texture, imageResult.CachedPathUri);
        }
    }
}
