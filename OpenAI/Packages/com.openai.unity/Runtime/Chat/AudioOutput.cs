// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Linq;
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
        public string Id { get; private set; }

        [Preserve]
        [JsonIgnore]
        public int? ExpiresAtUnixSeconds { get; private set; }

        [Preserve]
        [JsonIgnore]
        public DateTime? ExpiresAt => ExpiresAtUnixSeconds.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixSeconds.Value).DateTime
            : null;

        [Preserve]
        [JsonIgnore]
        public string Data { get; private set; }

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
        public string Transcript { get; private set; }

        [Preserve]
        public override string ToString() => Transcript ?? string.Empty;

        internal void AppendFrom(AudioOutput other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Id))
            {
                Id = other.Id;
            }

            if (other.ExpiresAtUnixSeconds.HasValue)
            {
                ExpiresAtUnixSeconds = other.ExpiresAtUnixSeconds;
            }

            if (!string.IsNullOrWhiteSpace(other.Transcript))
            {
                Transcript += other.Transcript;
            }

            if (!string.IsNullOrWhiteSpace(other.Data))
            {
                Data += other.Data;
            }
        }
    }
}
