// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    internal class ModalityConverter : JsonConverter<Modality>
    {
        [Preserve]
        public override void WriteJson(JsonWriter writer, Modality value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            if (value.HasFlag(Modality.Text)) { writer.WriteValue("text"); }
            if (value.HasFlag(Modality.Audio)) { writer.WriteValue("audio"); }
            writer.WriteEndArray();
        }

        [Preserve]
        public override Modality ReadJson(JsonReader reader, Type objectType, Modality existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var modalityArray = JArray.Load(reader);
            var modality = Modality.None;
            foreach (var modalityString in modalityArray)
            {
                modality |= modalityString.Value<string>() switch
                {
                    "text" => Modality.Text,
                    "audio" => Modality.Audio,
                    _ => throw new NotImplementedException($"Unknown modality: {modalityString}")
                };
            }
            return modality;
        }
    }
}
