// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class Content
    {
        [Preserve]
        [JsonConstructor]
        public Content(
            [JsonProperty("type")] ContentType contentType,
            [JsonProperty("text")] TextContent text,
            [JsonProperty("image_url")] ImageUrl imageUrl)
        {
            Type = contentType;
            Text = text;
            ImageUrl = imageUrl;
        }

        [Preserve]
        [JsonProperty("type")]
        public ContentType Type { get; }

        [Preserve]
        [JsonProperty("text")]
        public TextContent Text { get; }

        [Preserve]
        [JsonProperty("image_url")]
        public ImageUrl ImageUrl { get; }

        [Preserve]
        public override string ToString()
            => Type switch
            {
                ContentType.Text => Text.Value,
                ContentType.ImageUrl => ImageUrl.Url,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
