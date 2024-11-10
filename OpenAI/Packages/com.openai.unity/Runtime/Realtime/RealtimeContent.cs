// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeContent
    {
        [Preserve]
        [JsonConstructor]
        internal RealtimeContent(
            [JsonProperty("type")] RealtimeContentType type,
            [JsonProperty("text")] string text,
            [JsonProperty("audio")] string audio,
            [JsonProperty("transcript")] string transcript)
        {
            Type = type;
            Text = text;
            Audio = audio;
            Transcript = transcript;
        }

        [Preserve]
        public RealtimeContent(string text, RealtimeContentType type)
        {
            Type = type;
            Text = type switch
            {
                RealtimeContentType.InputText or RealtimeContentType.Text => text,
                _ => throw new ArgumentException($"Invalid content type {type} for text content")
            };
        }

        [Preserve]
        public RealtimeContent(AudioClip audioClip, RealtimeContentType type, string transcript = null)
        {
            Type = type;
            Audio = type switch
            {
                RealtimeContentType.InputAudio or RealtimeContentType.Audio => Convert.ToBase64String(audioClip.EncodeToPCM()),
                _ => throw new ArgumentException($"Invalid content type {type} for audio content")
            };
            Transcript = transcript;
        }

        [Preserve]
        public RealtimeContent(byte[] audioData, RealtimeContentType type, string transcript = null)
        {
            Type = type;
            Audio = type switch
            {
                RealtimeContentType.InputAudio or RealtimeContentType.Audio => Convert.ToBase64String(audioData),
                _ => throw new ArgumentException($"Invalid content type {type} for audio content")
            };
            Transcript = transcript;
        }

        /// <summary>
        /// The content type ("text", "audio", "input_text", "input_audio").
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public RealtimeContentType Type { get; }

        /// <summary>
        /// The text content.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// Base64-encoded audio data.
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public string Audio { get; }

        /// <summary>
        /// The transcript of the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; }

        [Preserve]
        public static implicit operator RealtimeContent(string text) => new(text, RealtimeContentType.InputText);

        [Preserve]
        public static implicit operator RealtimeContent(AudioClip audioClip) => new(audioClip, RealtimeContentType.InputAudio);

        [Preserve]
        public static implicit operator RealtimeContent(byte[] audioData) => new(audioData, RealtimeContentType.InputAudio);
    }
}
