// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Encoding.Wav;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class AudioContent : IResponseContent, IAppendable<AudioContent>
    {
        [Preserve]
        public AudioContent(AudioClip audioClip)
            : this(Convert.ToBase64String(audioClip.EncodeToWav()), InputAudioFormat.Wav)
        {
        }

        [Preserve]
        public AudioContent(byte[] data, InputAudioFormat format)
            : this($"data:audio/{format};base64,{Convert.ToBase64String(data)}", format)
        {
        }

        [Preserve]
        public AudioContent(string data, InputAudioFormat format)
        {
            Data = data;
            Format = format;
            Type = ResponseContentType.InputAudio;
        }

        [Preserve]
        [JsonConstructor]
        internal AudioContent(
            [JsonProperty("index")] int? index,
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("data")] string data,
            [JsonProperty("format")] InputAudioFormat format)
        {
            Index = index;
            Type = type;
            Data = data;
            Format = format;
        }

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("data")]
        public string Data { get; private set; }

        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Include)]
        public InputAudioFormat Format { get; private set; }

        [Preserve]
        public override string ToString() => Data;

        [Preserve]
        public void AppendFrom(AudioContent other)
        {
            if (other == null) { return; }

            if (other.Format > 0)
            {
                Format = other.Format;
            }

            if (!string.IsNullOrWhiteSpace(other.Data))
            {
                Data += other.Data;
            }
        }
    }
}
