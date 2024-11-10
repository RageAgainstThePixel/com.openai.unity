// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class AudioOutput
    {
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("expires_at")]
        public int ExpiresAtUnixSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixSeconds).DateTime;

        [Preserve]
        [JsonProperty("data")]
        public string Data { get; }

        [Preserve]
        [JsonIgnore]
        public AudioClip AudioClip
        {
            get
            {
                var samples = PCMEncoder.Decode(Convert.FromBase64String(Data));
                var audioClip = AudioClip.Create(Id, samples.Length, 1, 24000, false);
                audioClip.SetData(samples, 0);
                return audioClip;
            }
        }

        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; }
    }
}
