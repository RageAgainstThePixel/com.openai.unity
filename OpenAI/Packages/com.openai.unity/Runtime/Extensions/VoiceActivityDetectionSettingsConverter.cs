// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Realtime;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    internal class VoiceActivityDetectionSettingsConverter : JsonConverter
    {
        [Preserve]
        public override bool CanWrite => true;

        [Preserve]
        public override bool CanConvert(Type objectType) => typeof(IVoiceActivityDetectionSettings) == objectType;

        [Preserve]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "disabled" => new DisabledVAD(),
                "server_vad" => jObject.ToObject<ServerVAD>(serializer),
                "semantic_vad" => jObject.ToObject<SemanticVAD>(serializer),
                _ => throw new NotImplementedException($"Unknown VAD type: {type}")
            };
        }

        [Preserve]
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case DisabledVAD:
                    writer.WriteNull();
                    break;
                default:
                    serializer.Serialize(writer, value);
                    break;
            }
        }
    }
}
