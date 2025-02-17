// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Encoding.Wav;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeContent
    {
        [Preserve]
        [JsonConstructor]
        internal RealtimeContent(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] RealtimeContentType type,
            [JsonProperty("text")] string text,
            [JsonProperty("audio")] string audio,
            [JsonProperty("transcript")] string transcript)
        {
            Id = id;
            Type = type;
            Text = text;
            Audio = audio;
            Transcript = transcript;
        }

        [Preserve]
        public RealtimeContent(string text, RealtimeContentType type)
        {
            Type = type;
            switch (type)
            {
                case RealtimeContentType.InputText or RealtimeContentType.Text:
                    Text = text;
                    break;
                case RealtimeContentType.InputAudio or RealtimeContentType.Audio:
                    Audio = text;
                    break;
                case RealtimeContentType.ItemReference:
                    Id = text;
                    break;
                default:
                    throw new ArgumentException($"Invalid content type {type} for text content");
            }
        }

        [Preserve]
        public RealtimeContent(AudioClip audioClip, RealtimeContentType type, string transcript = null)
        {
            Type = type;
            Audio = type switch
            {
                RealtimeContentType.InputAudio or RealtimeContentType.Audio => $"data:audio/wav;base64,{Convert.ToBase64String(audioClip.EncodeToWav())}",
                _ => throw new ArgumentException($"Invalid content type {type} for audio content")
            };
            Transcript = transcript;
        }

        [Preserve]
        public RealtimeContent(ReadOnlyMemory<byte> audioData, RealtimeContentType type, string transcript = null)
            : this(audioData.Span, type, transcript)
        {
        }

        [Preserve]
        public RealtimeContent(ReadOnlySpan<byte> audioData, RealtimeContentType type, string transcript = null)
        {
            Type = type;
            Audio = type switch
            {
                RealtimeContentType.InputAudio or RealtimeContentType.Audio => Convert.ToBase64String(audioData),
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
        /// ID of a previous conversation item to reference (for `item_reference` content types in `response.create` events).
        /// These can reference both client and server created items.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The content type ("text", "audio", "input_text", "input_audio").
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public RealtimeContentType Type { get; }

        /// <summary>
        /// The text content, used for `input_text` and `text` content types.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        /// <summary>
        /// Base64-encoded audio bytes, used for `input_audio` content type.
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public string Audio { get; }

        /// <summary>
        /// The transcript of the audio, used for `input_audio` content type.
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
