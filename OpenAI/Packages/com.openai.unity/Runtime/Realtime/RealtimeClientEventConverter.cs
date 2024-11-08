// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Realtime
{
    internal class RealtimeClientEventConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();

        public override bool CanConvert(Type objectType) => typeof(IClientEvent) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "session.update" => jObject.ToObject<UpdateSessionRequest>(serializer),
                "input_audio_buffer.append" => jObject.ToObject<InputAudioBufferAppendRequest>(serializer),
                "input_audio_buffer.commit" => jObject.ToObject<InputAudioBufferCommitRequest>(serializer),
                "input_audio_buffer.clear" => jObject.ToObject<InputAudioBufferClearRequest>(serializer),
                "conversation.item.create" => jObject.ToObject<ConversationItemCreateRequest>(serializer),
                "conversation.item.truncate" => jObject.ToObject<ConversationItemTruncateRequest>(serializer),
                "conversation.item.delete" => jObject.ToObject<ConversationItemDeleteRequest>(serializer),
                "response.create" => jObject.ToObject<ResponseCreateRequest>(serializer),
                "response.cancel" => jObject.ToObject<ResponseCancelRequest>(serializer),
                _ => throw new NotImplementedException($"Unknown event type: {type}")
            };
        }
    }
}
