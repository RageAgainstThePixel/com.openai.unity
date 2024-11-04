// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Realtime
{
    internal class RealtimeModalityConverter : JsonConverter<RealtimeModality>
    {
        public override void WriteJson(JsonWriter writer, RealtimeModality value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            if (value.HasFlag(RealtimeModality.Text))
            {
                writer.WriteValue("text");
            }

            if (value.HasFlag(RealtimeModality.Audio))
            {
                writer.WriteValue("audio");
            }

            writer.WriteEndArray();
        }

        public override RealtimeModality ReadJson(JsonReader reader, Type objectType, RealtimeModality existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var modalityArray = JArray.Load(reader);
            var modality = RealtimeModality.None;
            foreach (var modalityString in modalityArray)
            {
                modality |= modalityString.Value<string>() switch
                {
                    "text" => RealtimeModality.Text,
                    "audio" => RealtimeModality.Audio,
                    _ => throw new NotImplementedException($"Unknown modality: {modalityString}")
                };
            }
            return modality;
        }
    }
}
