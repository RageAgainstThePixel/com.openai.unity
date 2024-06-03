// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// References an image URL in the content of a message.
    /// </summary>
    [Preserve]
    public sealed class ImageUrl
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url">
        /// The external URL of the image, must be a supported image types: jpeg, jpg, png, gif, webp.
        /// </param>
        /// <param name="detail">
        /// Specifies the detail level of the image if specified by the user.
        /// 'low' uses fewer tokens, you can opt in to high resolution using 'high'.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public ImageUrl(
            [JsonProperty("url")] string url,
            [JsonProperty("detail")] ImageDetail detail = ImageDetail.Auto)
        {
            Url = url;
            Detail = detail;
        }

        /// <summary>
        /// The external URL of the image, must be a supported image types: jpeg, jpg, png, gif, webp.
        /// </summary>
        [Preserve]
        [JsonProperty("url")]
        public string Url { get; private set; }

        /// <summary>
        /// Specifies the detail level of the image if specified by the user.
        /// 'low' uses fewer tokens, you can opt in to high resolution using 'high'.
        /// </summary>
        [Preserve]
        [JsonProperty("detail")]
        public ImageDetail Detail { get; private set; }

        public override string ToString() => Url;
    }
}
