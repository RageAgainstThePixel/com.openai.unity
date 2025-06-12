// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Extensions
{
    internal static class TextureExtensions
    {
        public static async Task<(Texture2D, string)> ConvertFromBase64Async(string data, bool debug, CancellationToken cancellationToken)
        {
            var imageData = Convert.FromBase64String(data);
#if PLATFORM_WEBGL
            var texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            return await Task.FromResult((texture, string.Empty));
#else
            if (!Rest.TryGetDownloadCacheItem(data, out var localFilePath))
            {
                localFilePath = localFilePath.GetPathSafeString();
                await File.WriteAllBytesAsync(localFilePath, imageData, cancellationToken).ConfigureAwait(true);
                localFilePath = $"file://{localFilePath}";
            }

            var texture = await Rest.DownloadTextureAsync(localFilePath, parameters: new RestParameters(debug: debug), cancellationToken: cancellationToken);
            Rest.TryGetDownloadCacheItem(data, out var cachedPath);
            return (texture, cachedPath);
#endif
        }
    }
}
