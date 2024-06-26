// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using UnityEngine.Scripting;

namespace OpenAI
{
    /// <summary>
    /// References an image URL in the content of a message.
    /// </summary>
    [Preserve]
    public sealed class ImageUrl : IAppendable<ImageUrl>
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
        public ImageUrl(string url, ImageDetail detail = ImageDetail.Auto)
        {
            Url = url;
            Detail = detail;
        }

        [Preserve]
        [JsonConstructor]
        internal ImageUrl(
            [JsonProperty("index")] int? index,
            [JsonProperty("url")] string url,
            [JsonProperty("detail")] ImageDetail detail)
        {
            Index = index;
            Url = url;
            Detail = detail;
        }

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; private set; }

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

        public void AppendFrom(ImageUrl other)
        {
            if (other == null) { return; }

            if (other.Index.HasValue)
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other.Url))
            {
                Url += other.Url;
            }

            if (other.Detail > 0)
            {
                Detail = other.Detail;
            }
        }
    }
}
