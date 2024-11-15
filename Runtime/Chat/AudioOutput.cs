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
        [JsonConstructor]
        internal AudioOutput(
            [JsonProperty("id")] string id,
            [JsonProperty("expires_at")] int expiresAtUnixSeconds,
            [JsonProperty("data")] string data,
            [JsonProperty("transcript")] string transcript)
        {
            Id = id;
            ExpiresAtUnixSeconds = expiresAtUnixSeconds;
            Data = data;
            Transcript = transcript;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonIgnore]
        public int ExpiresAtUnixSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime ExpiresAt => DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixSeconds).DateTime;

        [Preserve]
        [JsonIgnore]
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
        [JsonIgnore]
        public string Transcript { get; }

        [Preserve]
        public override string ToString() => Transcript ?? string.Empty;
    }
}
