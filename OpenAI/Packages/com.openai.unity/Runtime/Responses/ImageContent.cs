// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ImageContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal ImageContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("detail")] ImageDetail detail,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("image_url")] string imageUrl)
        {
            Type = type;
            Detail = detail;
            FileId = fileId;
            ImageUrl = imageUrl;
        }

        [Preserve]
        public ImageContent(string imageUrl = null, string fileId = null, ImageDetail detail = ImageDetail.Auto)
        {
            Type = ResponseContentType.InputImage;
            ImageUrl = imageUrl;
            FileId = fileId;
            Detail = detail;
        }

        [Preserve]
        public ImageContent(Texture2D image, ImageDetail detail = ImageDetail.Auto)
        {
            Type = ResponseContentType.InputImage;
            Detail = detail;
            ImageUrl = $"data:image/png;base64,{System.Convert.ToBase64String(image.EncodeToPNG())}";
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("detail", DefaultValueHandling = DefaultValueHandling.Include)]
        public ImageDetail Detail { get; }

        [Preserve]
        [JsonProperty("file_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileId { get; }

        [Preserve]
        [JsonProperty("image_url")]
        public string ImageUrl { get; }

        [Preserve]
        public static implicit operator ImageContent(Texture2D image) => new(image);

        [Preserve]
        public static implicit operator ImageContent(string imageUrl) => new(imageUrl);
    }
}
