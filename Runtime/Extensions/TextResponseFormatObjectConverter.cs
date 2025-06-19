// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Extensions
{
    [Preserve]
    internal sealed class TextResponseFormatObjectConverter : JsonConverter<TextResponseFormatConfiguration>
    {
        [Preserve]
        public override TextResponseFormatConfiguration ReadJson(JsonReader reader, Type objectType, TextResponseFormatConfiguration existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return TextResponseFormat.Auto;
                }

                if (reader.TokenType == JsonToken.String)
                {
                    return serializer.Deserialize<TextResponseFormat>(reader);
                }

                var jsonObject = JObject.Load(reader);

                if (jsonObject.TryGetValue("type", out var typeProperty) &&
                    typeProperty.Type == JTokenType.String)
                {
                    var type = typeProperty.ToObject<TextResponseFormat>(serializer);

                    if (type == TextResponseFormat.JsonSchema)
                    {
                        if (!jsonObject.TryGetValue("schema", out var schemaProperty) || schemaProperty.Type != JTokenType.Object)
                        {
                            throw new ArgumentException("JsonSchema must be provided when using JsonSchema response format.");
                        }

                        var jsonSchema = schemaProperty.ToObject<JsonSchema>(serializer);

                        return new TextResponseFormatConfiguration(jsonSchema);
                    }

                    return type;
                }

                throw new ArgumentException($"Invalid JSON format for TextResponseFormatConfiguration.\n{jsonObject}");
            }
            catch (Exception e)
            {
                throw new JsonSerializationException($"Error reading {nameof(TextResponseFormat)} from JSON", e);
            }
        }

        [Preserve]
        public override void WriteJson(JsonWriter writer, TextResponseFormatConfiguration value, JsonSerializer serializer)
        {
            switch (value.Type)
            {
                case TextResponseFormat.Auto:
                    // ignore
                    break;
                case TextResponseFormat.JsonSchema:
                    if (value.JsonSchema == null)
                    {
                        throw new ArgumentException("JsonSchema must be provided when using JsonSchema response format.");
                    }
                    serializer.Serialize(writer, new
                    {
                        type = value.Type,
                        name = value.JsonSchema.Name,
                        strict = value.JsonSchema.Strict,
                        schema = value.JsonSchema.Schema
                    });
                    break;
#pragma warning disable CS0618 // Type or member is obsolete
                case TextResponseFormat.Json:
#pragma warning restore CS0618 // Type or member is obsolete
                case TextResponseFormat.Text:
                    serializer.Serialize(writer, new { type = value.Type });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value.Type), $"Unsupported response format: {value.Type}");
            }
        }
    }
}
