// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    internal sealed class RealtimeContentTypeConverter : JsonConverter<RealtimeContentType>
    {
        [Preserve]
        public override void WriteJson(JsonWriter writer, RealtimeContentType value, JsonSerializer serializer)
        {
            var stringValue = value switch
            {
                RealtimeContentType.OutputText => "output_text",
                RealtimeContentType.OutputAudio => "output_audio",
                RealtimeContentType.InputText => "input_text",
                RealtimeContentType.InputAudio => "input_audio",
                RealtimeContentType.ItemReference => "item_reference",
                _ => throw new NotImplementedException($"Unknown content type: {value}")
            };

            writer.WriteValue(stringValue);
        }

        [Preserve]
        public override RealtimeContentType ReadJson(
            JsonReader reader,
            Type objectType,
            RealtimeContentType existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing content type.");
            }

            return reader.Value?.ToString() switch
            {
                "text" => RealtimeContentType.OutputText,
                "audio" => RealtimeContentType.OutputAudio,
                "output_text" => RealtimeContentType.OutputText,
                "output_audio" => RealtimeContentType.OutputAudio,
                "input_text" => RealtimeContentType.InputText,
                "input_audio" => RealtimeContentType.InputAudio,
                "item_reference" => RealtimeContentType.ItemReference,
                var value => throw new JsonSerializationException($"Unknown content type: {value}")
            };
        }
    }
}
