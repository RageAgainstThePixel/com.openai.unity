// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class InputImageMask
    {
        [Preserve]
        [JsonConstructor]
        public InputImageMask(string imageUrl = null, string fileId = null)
        {
            ImageUrl = imageUrl;
            FileId = fileId;
        }

        /// <summary>
        /// Base64-encoded mask image.
        /// </summary>
        [Preserve]
        [JsonProperty("image_url")]
        public string ImageUrl { get; }

        /// <summary>
        /// File ID for the mask image.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }
    }
}
