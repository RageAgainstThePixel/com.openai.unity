// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Extensions
{
    [Preserve]
    internal sealed class StringOrObjectConverter<T> : JsonConverter
    {
        [Preserve]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => reader.TokenType switch
            {
                JsonToken.Null => null,
                JsonToken.String => reader.Value,
                JsonToken.StartObject => serializer.Deserialize<T>(reader),
                JsonToken.StartArray => serializer.Deserialize<T>(reader),
                _ => throw new JsonSerializationException($"Unexpected token {reader.TokenType} when reading {typeof(T)}")
            };

        [Preserve]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case null:
                    writer.WriteNull();
                    break;
                case string stringValue:
                    writer.WriteValue(stringValue);
                    break;
                case T objectValue:
                    serializer.Serialize(writer, objectValue);
                    break;
                default:
                    throw new JsonSerializationException($"Unexpected value type {value.GetType()}");
            }
        }

        [Preserve]
        public override bool CanConvert(Type objectType) => true;
    }
}
