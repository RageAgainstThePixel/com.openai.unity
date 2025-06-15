// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ComputerScreenShot
    {
        [Preserve]
        [JsonConstructor]
        internal ComputerScreenShot(
            [JsonProperty("type")] string type,
            [JsonProperty("image_url")] string imageUrl,
            [JsonProperty("file_id")] string fileId)
        {
            Type = type;
            ImageUrl = imageUrl;
            FileId = fileId;
        }

        /// <summary>
        /// Specifies the event type. For a computer screenshot, this property is always set to `computer_screenshot`.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// The URL of the screenshot image.
        /// </summary>
        [Preserve]
        [JsonProperty("image_url")]
        public string ImageUrl { get; }

        /// <summary>
        /// The identifier of an uploaded file that contains the screenshot.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }
    }
}
