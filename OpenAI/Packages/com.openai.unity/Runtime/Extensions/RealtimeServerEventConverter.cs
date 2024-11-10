// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Realtime;
using System;

namespace OpenAI
{
    internal class RealtimeServerEventConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();

        public override bool CanConvert(Type objectType) => typeof(IServerEvent) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "error" => jObject.ToObject<RealtimeEventError>(serializer),
                _ when type.StartsWith("session") => jObject.ToObject<SessionResponse>(serializer),
                "conversation.created" => jObject.ToObject<RealtimeConversationResponse>(serializer),
                "conversation.item.created" => jObject.ToObject<ConversationItemCreatedResponse>(serializer),
                _ when type.StartsWith("conversation.item.input_audio_transcription") => jObject.ToObject<ConversationItemInputAudioTranscriptionResponse>(serializer),
                "conversation.item.truncated" => jObject.ToObject<ConversationItemTruncatedResponse>(serializer),
                "conversation.item.deleted" => jObject.ToObject<ConversationItemDeletedResponse>(serializer),
                "input_audio_buffer.committed" => jObject.ToObject<InputAudioBufferCommittedResponse>(serializer),
                "input_audio_buffer.cleared" => jObject.ToObject<InputAudioBufferClearedResponse>(serializer),
                "input_audio_buffer.speech_started" => jObject.ToObject<InputAudioBufferStartedResponse>(serializer),
                "input_audio_buffer.speech_stopped" => jObject.ToObject<InputAudioBufferStoppedResponse>(serializer),
                _ when type.StartsWith("response.audio_transcript") => jObject.ToObject<ResponseAudioTranscriptResponse>(serializer),
                _ when type.StartsWith("response.audio") => jObject.ToObject<ResponseAudioResponse>(),
                _ when type.StartsWith("response.content_part") => jObject.ToObject<ResponseContentPartResponse>(serializer),
                _ when type.StartsWith("response.function_call_arguments") => jObject.ToObject<ResponseFunctionCallArguments>(serializer),
                _ when type.StartsWith("response.output_item") => jObject.ToObject<ResponseOutputItemResponse>(serializer),
                _ when type.StartsWith("response.text") => jObject.ToObject<ResponseTextResponse>(serializer),
                _ when type.StartsWith("response") => jObject.ToObject<RealtimeResponse>(serializer),
                _ when type.StartsWith("rate_limits") => jObject.ToObject<RateLimitsResponse>(serializer),
                _ => throw new NotImplementedException($"Unknown event type: {type}")
            };
        }
    }
}
