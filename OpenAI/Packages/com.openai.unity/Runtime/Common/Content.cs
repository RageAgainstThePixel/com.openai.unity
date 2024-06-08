// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Content : IAppendable<Content>
    {
        [Preserve]
        public Content() { }

        [Preserve]
        public Content(string text)
            : this(ContentType.Text, text)
        {
        }

        [Preserve]
        public Content(TextContent textContent)
        {
            Type = ContentType.Text;
            Text = textContent;
        }

        [Preserve]
        public Content(ImageUrl imageUrl)
        {
            Type = ContentType.ImageUrl;
            ImageUrl = imageUrl;
        }

        [Preserve]
        public Content(Texture2D texture)
            : this(ContentType.ImageUrl, $"data:image/png;base64,{Convert.ToBase64String(texture.EncodeToPNG())}")
        {
        }

        [Preserve]
        public Content(ImageFile imageFile)
        {
            Type = ContentType.ImageFile;
            ImageFile = imageFile;
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
                case ContentType.ImageFile:
                    throw new ArgumentException("Use the ImageFile constructor for ImageFile content.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        [Preserve]
        [JsonConstructor]
        internal Content(
            [JsonProperty("index")] int? index,
            [JsonProperty("type")] ContentType type,
            [JsonProperty("text")] object text,
            [JsonProperty("image_url")] ImageUrl imageUrl,
            [JsonProperty("image_file")] ImageFile imageFile)
        {
            Index = index;
            Type = type;
            Text = text;
            ImageUrl = imageUrl;
            ImageFile = imageFile;
        }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
        public ContentType Type { get; private set; }

        [Preserve]
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringOrObjectConverter<TextContent>))]
        public object Text { get; private set; }

        [Preserve]
        [JsonProperty("image_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageUrl ImageUrl { get; private set; }

        [Preserve]
        [JsonProperty("image_file", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageFile ImageFile { get; private set; }

        [Preserve]
        public static implicit operator Content(string input) => new(ContentType.Text, input);

        [Preserve]
        public static implicit operator Content(ImageUrl imageUrl) => new(imageUrl);

        [Preserve]
        public static implicit operator Content(Texture2D texture) => new(texture);

        [Preserve]
        public static implicit operator Content(ImageFile imageFile) => new(imageFile);

        public override string ToString()
            => Type switch
            {
                ContentType.Text => Text?.ToString(),
                ContentType.ImageUrl => ImageUrl?.ToString(),
                ContentType.ImageFile => ImageFile?.ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(Type))
            } ?? string.Empty;

        public void AppendFrom(Content other)
        {
            if (other == null) { return; }

            if (other.Type > 0)
            {
                Type = other.Type;
            }

            if (other.Text is TextContent otherTextContent)
            {
                if (Text is TextContent textContent)
                {
                    textContent.AppendFrom(otherTextContent);
                }
                else
                {
                    Text = otherTextContent;
                }
            }
            else if (other.Text is string otherStringContent)
            {
                if (!string.IsNullOrWhiteSpace(otherStringContent))
                {
                    Text += otherStringContent;
                }
            }

            if (other.ImageUrl != null)
            {
                if (ImageUrl == null)
                {
                    ImageUrl = other.ImageUrl;
                }
                else
                {
                    ImageUrl.AppendFrom(other.ImageUrl);
                }
            }

            if (other.ImageFile != null)
            {
                if (ImageFile == null)
                {
                    ImageFile = other.ImageFile;
                }
                else
                {
                    ImageFile.AppendFrom(other.ImageFile);
                }
            }
        }
    }
}
