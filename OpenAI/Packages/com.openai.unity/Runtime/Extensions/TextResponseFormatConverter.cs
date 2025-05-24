// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Extensions
{
    [Preserve]
    internal sealed class TextResponseFormatConverter : JsonConverter<TextResponseFormatConfiguration>
    {
        [Preserve]
        public override TextResponseFormatConfiguration ReadJson(JsonReader reader, Type objectType, TextResponseFormatConfiguration existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType is JsonToken.Null or JsonToken.String)
                {
                    return ChatResponseFormat.Auto;
                }

                return serializer.Deserialize<TextResponseFormatConfiguration>(reader);
            }
            catch (Exception e)
            {
                throw new JsonSerializationException($"Error reading {nameof(ChatResponseFormat)} from JSON", e);
            }
        }

        [Preserve]
        public override void WriteJson(JsonWriter writer, TextResponseFormatConfiguration value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
