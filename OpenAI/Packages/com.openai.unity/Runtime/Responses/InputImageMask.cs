// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Responses
{
    public sealed class InputImageMask
    {
        [JsonConstructor]
        public InputImageMask(string imageUrl = null, string fileId = null)
        {
            ImageUrl = imageUrl;
            FileId = fileId;
        }

        /// <summary>
        /// Base64-encoded mask image.
        /// </summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; }

        /// <summary>
        /// File ID for the mask image.
        /// </summary>
        [JsonProperty("file_id")]
        public string FileId { get; }
    }
}
