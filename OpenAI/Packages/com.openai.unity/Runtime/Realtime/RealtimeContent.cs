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

        public RealtimeContent(string text)
        {
            Type = RealtimeContentType.InputText;
            Text = text;
        }

        public RealtimeContent(AudioClip audioClip)
        {
            Type = RealtimeContentType.InputAudio;
            Audio = Convert.ToBase64String(audioClip.EncodeToPCM());
        }

        public RealtimeContent(byte[] audioData)
        {
            Type = RealtimeContentType.InputAudio;
            Audio = Convert.ToBase64String(audioData);
        }

        /// <summary>
        /// The content type ("text", "audio", "input_text", "input_audio").
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public RealtimeContentType Type { get; private set; }

        /// <summary>
        /// The text content.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Base64-encoded audio data.
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public string Audio { get; private set; }

        /// <summary>
        /// The transcript of the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; private set; }

        [Preserve]
        public static implicit operator RealtimeContent(string text) => new(text);
    }
}
