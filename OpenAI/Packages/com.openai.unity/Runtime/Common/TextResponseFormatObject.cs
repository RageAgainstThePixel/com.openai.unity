// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TextResponseFormatObject
    {
        [Preserve]
        [JsonConstructor]
        public TextResponseFormatObject(
            [JsonProperty("format")]
            [JsonConverter(typeof(TextResponseFormatObjectConverter))]
            TextResponseFormatConfiguration format)
        {
            Format = format ?? throw new ArgumentNullException(nameof(format), "Format cannot be null.");
        }

        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(TextResponseFormatObjectConverter))]
        public TextResponseFormatConfiguration Format { get; private set; }

        public static implicit operator TextResponseFormatObject(TextResponseFormatConfiguration config) => new(config);

        public static implicit operator TextResponseFormat(TextResponseFormatObject formatObj) => formatObj?.Format?.Type ?? TextResponseFormat.Auto;

        public static implicit operator TextResponseFormatObject(TextResponseFormat format) => new(new(format));

        public static implicit operator TextResponseFormatObject(JsonSchema schema) => new(new(schema));
    }
}
