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
            [JsonProperty("image_file")] ImageFile imageUrl)
        {
            Type = contentType;
            Text = text;
            ImageFile = imageUrl;
        }

        [Preserve]
        [JsonProperty("type")]
        public ContentType Type { get; }

        [Preserve]
        [JsonProperty("text")]
        public TextContent Text { get; }

        [Preserve]
        [JsonProperty("image_file")]
        public ImageFile ImageFile { get; }

        [Preserve]
        public override string ToString()
            => Type switch
            {
                ContentType.Text => Text.Value,
                ContentType.ImageFile => ImageFile.FileId,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
