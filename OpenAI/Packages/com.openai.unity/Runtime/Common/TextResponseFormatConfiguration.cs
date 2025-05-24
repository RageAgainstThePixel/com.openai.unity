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
        public TextResponseFormatConfiguration(ChatResponseFormat type)
        {
            if (type == ChatResponseFormat.JsonSchema)
            {
                throw new ArgumentException("Use the constructor overload that accepts a JsonSchema object for ChatResponseFormat.JsonSchema.", nameof(type));
            }
            Type = type;
        }

        [Preserve]
        public TextResponseFormatConfiguration(JsonSchema schema)
        {
            Type = ChatResponseFormat.JsonSchema;
            JsonSchema = schema;
        }

        [Preserve]
        [JsonConstructor]
        internal TextResponseFormatConfiguration(
            [JsonProperty("type")] ChatResponseFormat type,
            [JsonProperty("json_schema")] JsonSchema schema)
        {
            Type = type;
            JsonSchema = schema;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ChatResponseFormat Type { get; private set; }

        [Preserve]
        [JsonProperty("json_schema", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JsonSchema JsonSchema { get; private set; }

        public static implicit operator TextResponseFormatConfiguration(ChatResponseFormat type) => new(type);

        [Preserve]
        public static implicit operator ChatResponseFormat(TextResponseFormatConfiguration format) => format.Type;
    }
}
