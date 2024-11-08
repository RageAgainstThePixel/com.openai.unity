﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to commit the user input audio buffer,
    /// which will create a new user message item in the conversation.
    /// This event will produce an error if the input audio buffer is empty.
    /// When in Server VAD mode, the client does not need to send this event,
    /// the server will commit the audio buffer automatically.
    /// Committing the input audio buffer will trigger input audio transcription (if enabled in session configuration),
    /// but it will not create a response from the model.
    /// The server will respond with an input_audio_buffer.committed event.
    /// </summary>
    [Preserve]
    public sealed class InputAudioBufferCommitRequest : BaseRealtimeEvent, IClientEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "input_audio_buffer.commit";
    }
}
