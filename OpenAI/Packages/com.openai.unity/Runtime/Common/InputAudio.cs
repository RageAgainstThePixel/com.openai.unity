// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Encoding.Wav;

namespace OpenAI
{
    [Preserve]
    public sealed class InputAudio
    {
        [Preserve]
        public InputAudio(AudioClip audioClip)
            : this(Convert.ToBase64String(audioClip.EncodeToWav()), InputAudioFormat.Wav)
        {
        }

        [Preserve]
        public InputAudio(byte[] data, InputAudioFormat format)
            : this(Convert.ToBase64String(data), format)
        {
        }

        [Preserve]
        public InputAudio(string data, InputAudioFormat format)
        {
            Data = data;
            Format = format;
        }

        [Preserve]
        [JsonProperty("data")]
        public string Data { get; private set; }

        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Include)]
        public InputAudioFormat Format { get; private set; }
    }
}
