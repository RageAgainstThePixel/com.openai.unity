// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Realtime;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    internal class VoiceActivityDetectionSettingsConverter : JsonConverter<VoiceActivityDetectionSettings>
    {
        [Preserve]
        public override VoiceActivityDetectionSettings ReadJson(JsonReader reader, Type objectType, VoiceActivityDetectionSettings existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.Null
                ? VoiceActivityDetectionSettings.Disabled()
                : serializer.Deserialize<VoiceActivityDetectionSettings>(reader);
        }

        [Preserve]
        public override void WriteJson(JsonWriter writer, VoiceActivityDetectionSettings value, JsonSerializer serializer)
        {
            switch (value.Type)
            {
                case TurnDetectionType.Disabled:
                    writer.WriteNull();
                    break;
                default:
                case TurnDetectionType.Server_VAD:
                    serializer.Serialize(writer, value);
                    break;
            }
        }
    }
}
