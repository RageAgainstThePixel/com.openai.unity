// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TextResponseFormatConfiguration
    {
        [Preserve]
        public TextResponseFormatConfiguration() { }

        [Preserve]
        [Obsolete("Use new overload with TextResponseFormat instead")]
        public TextResponseFormatConfiguration(ChatResponseFormat type)
        {
            if (type == ChatResponseFormat.JsonSchema)
            {
                throw new ArgumentException("Use the constructor overload that accepts a JsonSchema object for ChatResponseFormat.JsonSchema.", nameof(type));
            }

            Type = type switch
            {
                ChatResponseFormat.Text => TextResponseFormat.Text,
                ChatResponseFormat.Json => TextResponseFormat.Json,
                _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported response format: {type}")
            };
        }

        [Preserve]
        public TextResponseFormatConfiguration(TextResponseFormat type)
        {
            if (type == TextResponseFormat.JsonSchema)
            {
                throw new ArgumentException("Use the constructor overload that accepts a JsonSchema object for TextResponseFormat.JsonSchema.", nameof(type));
            }

            Type = type;
        }

        [Preserve]
        public TextResponseFormatConfiguration(JsonSchema schema)
        {
            Type = TextResponseFormat.JsonSchema;
            JsonSchema = schema;
        }

        [Preserve]
        [JsonConstructor]
        internal TextResponseFormatConfiguration(
            [JsonProperty("type")] TextResponseFormat type,
            [JsonProperty("json_schema")] JsonSchema schema)
        {
            Type = type;
            JsonSchema = schema;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public TextResponseFormat Type { get; private set; }

        [Preserve]
        [JsonProperty("json_schema", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JsonSchema JsonSchema { get; private set; }

        [Preserve]
        public static implicit operator TextResponseFormatConfiguration(TextResponseFormat type) => new(type);

        [Preserve]
        public static implicit operator TextResponseFormat(TextResponseFormatConfiguration format) => format.Type;

#pragma warning disable CS0618 // Type or member is obsolete
        [Preserve]
        [Obsolete("Use new overload with TextResponseFormat instead")]
        public static implicit operator TextResponseFormatConfiguration(ChatResponseFormat type) => new(type);

        [Preserve]
        [Obsolete("Use new overload with TextResponseFormat instead")]
        public static implicit operator ChatResponseFormat(TextResponseFormatConfiguration format)
        {
            return format.Type switch
            {
                TextResponseFormat.Text => ChatResponseFormat.Text,
                TextResponseFormat.Json => ChatResponseFormat.Json,
                TextResponseFormat.JsonSchema => ChatResponseFormat.JsonSchema,
                _ => throw new ArgumentOutOfRangeException(nameof(format), $"Unsupported response format: {format.Type}")
            };
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
