// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class Content
    {
        [Preserve]
        public Content(string text)
            : this(ContentType.Text, text)
        {
        }

        [Preserve]
        public Content(Texture2D texture)
            : this(ContentType.ImageUrl, $"data:image/png;base64,{Convert.ToBase64String(texture.EncodeToPNG())}")
        {
        }

        [Preserve]
        public Content(ImageUrl imageUrl)
        {
            Type = ContentType.ImageUrl;
            ImageUrl = imageUrl;
        }

        [Preserve]
        public Content(ContentType type, string input)
        {
            Type = type;

            switch (Type)
            {
                case ContentType.Text:
                    Text = input;
                    break;
                case ContentType.ImageUrl:
                    ImageUrl = new ImageUrl(input);
                    break;
            }
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
        public ContentType Type { get; private set; }

        [Preserve]
        [JsonProperty("text")]
        public string Text { get; private set; }

        [Preserve]
        [JsonProperty("image_url")]
        public ImageUrl ImageUrl { get; private set; }

        [Preserve]
        public static implicit operator Content(string input) => new(ContentType.Text, input);

        [Preserve]
        public static implicit operator Content(ImageUrl imageUrl) => new(imageUrl);

        [Preserve]
        public static implicit operator Content(Texture2D texture) => new(texture);
    }
}
