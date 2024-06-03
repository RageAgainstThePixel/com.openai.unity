// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Extensions
{
    [Preserve]
    internal sealed class ResponseFormatConverter : JsonConverter<ChatResponseFormat>
    {
        [Preserve]
        private sealed class ResponseFormatObject
        {
            [Preserve]
            public ResponseFormatObject() => Type = ChatResponseFormat.Text;

            [Preserve]
            [JsonConstructor]
            public ResponseFormatObject([JsonProperty("type")] ChatResponseFormat type) => Type = type;

            [Preserve]
            [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
            public ChatResponseFormat Type { get; private set; }

            [Preserve]
            public static implicit operator ResponseFormatObject(ChatResponseFormat type) => new(type);

            [Preserve]
            public static implicit operator ChatResponseFormat(ResponseFormatObject format) => format.Type;
        }

        [Preserve]
        public override ChatResponseFormat ReadJson(JsonReader reader, Type objectType, ChatResponseFormat existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType is JsonToken.Null or JsonToken.String)
                {
                    return ChatResponseFormat.Auto;
                }

                return serializer.Deserialize<ResponseFormatObject>(reader);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException($"Error reading {nameof(ChatResponseFormat)} from JSON", e);

            }
        }

        [Preserve]
        public override void WriteJson(JsonWriter writer, ChatResponseFormat value, JsonSerializer serializer)
        {
            const string type = nameof(type);
            const string text = nameof(text);
            // ReSharper disable once InconsistentNaming
            const string json_object = nameof(json_object);

            switch (value)
            {
                case ChatResponseFormat.Auto:
                    writer.WriteNull();
                    break;
                case ChatResponseFormat.Text:
                    writer.WriteStartObject();
                    writer.WritePropertyName(type);
                    writer.WriteValue(text);
                    writer.WriteEndObject();
                    break;
                case ChatResponseFormat.Json:
                    writer.WriteStartObject();
                    writer.WritePropertyName(type);
                    writer.WriteValue(json_object);
                    writer.WriteEndObject();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
