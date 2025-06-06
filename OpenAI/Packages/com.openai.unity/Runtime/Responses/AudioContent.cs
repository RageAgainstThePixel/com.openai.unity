// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Encoding.Wav;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class AudioContent : BaseResponse, IResponseContent
    {
        [Preserve]
        public AudioContent(AudioClip audioClip)
            : this(Convert.ToBase64String(audioClip.EncodeToWav()), InputAudioFormat.Wav)
        {
        }

        [Preserve]
        public AudioContent(ReadOnlyMemory<byte> memory, InputAudioFormat format)
            : this(memory.Span, format)
        {
        }

        [Preserve]
        public AudioContent(ReadOnlySpan<byte> span, InputAudioFormat format)
            : this($"data:audio/{format};base64,{Convert.ToBase64String(span)}", format)
        {
        }

        [Preserve]
        public AudioContent(byte[] data, InputAudioFormat format)
            : this($"data:audio/{format};base64,{Convert.ToBase64String(data)}", format)
        {
        }

        [Preserve]
        public AudioContent(string base64Data, InputAudioFormat format)
        {
            Base64Data = base64Data;
            Format = format;
            Type = ResponseContentType.InputAudio;
        }

        [Preserve]
        [JsonConstructor]
        internal AudioContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("data")] string base64Data = null,
            [JsonProperty("format")] InputAudioFormat format = 0,
            [JsonProperty("transcript")] string transcript = null)
        {
            Type = type;
            Base64Data = base64Data;

            if (!string.IsNullOrWhiteSpace(base64Data))
            {
                data = Convert.FromBase64String(base64Data);
            }

            Format = format;
            Transcript = transcript;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; private set; }

        [Preserve]
        [JsonProperty("data")]
        public string Base64Data { get; private set; }

        private Memory<byte> data = Memory<byte>.Empty;

        [JsonIgnore]
        public ReadOnlyMemory<byte> Data => data;

        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Include)]
        public InputAudioFormat Format { get; private set; }

        [Preserve]
        [JsonProperty("transcript", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Transcript { get; private set; }

        [JsonIgnore]
        public string Object => Type.ToString();

        internal void AppendFrom(AudioContent other)
        {
            if (other == null)
            {
                return;
            }

            if (other.Type > 0)
            {
                Type = other.Type;
            }

            if (!string.IsNullOrWhiteSpace(other.Base64Data))
            {
                Base64Data += other.Base64Data;
            }

            if (other.Data.Length > 0)
            {
                data = data.ToArray().Concat(other.Data.ToArray()).ToArray();
            }

            if (other.Format > 0)
            {
                Format = other.Format;
            }

            if (!string.IsNullOrWhiteSpace(other.Transcript))
            {
                Transcript += other.Transcript;
            }
        }
    }
}
