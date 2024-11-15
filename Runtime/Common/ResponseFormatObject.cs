// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ResponseFormatObject
    {
        [Preserve]
        public ResponseFormatObject() { }

        [Preserve]
        public ResponseFormatObject(ChatResponseFormat type)
        {
            if (type == ChatResponseFormat.JsonSchema)
            {
                throw new System.ArgumentException("Use the constructor overload that accepts a JsonSchema object for ChatResponseFormat.JsonSchema.", nameof(type));
            }
            Type = type;
        }

        [Preserve]
        public ResponseFormatObject(JsonSchema schema)
        {
            Type = ChatResponseFormat.JsonSchema;
            JsonSchema = schema;
        }

        [Preserve]
        [JsonConstructor]
        internal ResponseFormatObject(
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

        public static implicit operator ResponseFormatObject(ChatResponseFormat type) => new(type);

        [Preserve]
        public static implicit operator ChatResponseFormat(ResponseFormatObject format) => format.Type;
    }
}
