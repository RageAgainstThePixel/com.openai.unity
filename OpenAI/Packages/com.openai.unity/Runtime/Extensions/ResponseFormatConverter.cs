// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Extensions
{
    [Preserve]
    internal sealed class ResponseFormatConverter : JsonConverter<ResponseFormatObject>
    {
        [Preserve]
        public override ResponseFormatObject ReadJson(JsonReader reader, Type objectType, ResponseFormatObject existingValue, bool hasExistingValue, JsonSerializer serializer)
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
        public override void WriteJson(JsonWriter writer, ResponseFormatObject value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
