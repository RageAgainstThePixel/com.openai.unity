﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to append audio bytes to the input audio buffer.
    /// The audio buffer is temporary storage you can write to and later commit.
    /// In Server VAD mode, the audio buffer is used to detect speech and the server will decide when to commit.
    /// When Server VAD is disabled, you must commit the audio buffer manually.
    /// The client may choose how much audio to place in each event up to a maximum of 15 MiB,
    /// for example streaming smaller chunks from the client may allow the VAD to be more responsive.
    /// Unlike made other client events, the server will not send a confirmation response to this event.
    /// </summary>
    /// <remarks>Sample rate must be 24000</remarks>
    [Preserve]
    public sealed class InputAudioBufferAppendRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public InputAudioBufferAppendRequest(AudioClip audioClip)
            => Audio = Convert.ToBase64String(audioClip.EncodeToPCM(outputSampleRate: 24000));

        [Preserve]
        public InputAudioBufferAppendRequest(ReadOnlyMemory<byte> audioData)
            : this(audioData.Span)
        {
        }

        [Preserve]
        public InputAudioBufferAppendRequest(ReadOnlySpan<byte> audioData)
        {
            Audio = Convert.ToBase64String(audioData);
        }

        [Preserve]
        public InputAudioBufferAppendRequest(byte[] audioData)
        {
            Audio = Convert.ToBase64String(audioData);
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "input_audio_buffer.append";

        /// <summary>
        /// Base64-encoded audio bytes.
        /// This must be in the format specified by the input_audio_format field in the session configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public string Audio { get; }
    }
}
