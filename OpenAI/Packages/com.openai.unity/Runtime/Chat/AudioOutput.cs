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
        public ReadOnlyMemory<byte> AudioData
        {
            get
            {
                if (audioData.Length == 0)
                {
                    audioData = Convert.FromBase64String(Data);
                }

                return audioData;
            }
        }
        private Memory<byte> audioData;

        [Preserve]
        [JsonIgnore]
        public string Data { get; private set; }

        [Preserve]
        [JsonIgnore]
        public float[] AudioSamples
            => audioSamples ??= PCMEncoder.Decode(AudioData.ToArray(), inputSampleRate: 24000, outputSampleRate: AudioSettings.outputSampleRate);

        private float[] audioSamples;

        [Preserve]
        [JsonIgnore]
        public float Length => AudioSamples.Length / (float)AudioSettings.outputSampleRate;

        [Preserve]
        [JsonIgnore]
        public AudioClip AudioClip
        {
            get
            {
                if (audioClip == null)
                {
                    audioClip = AudioClip.Create(Id, AudioSamples.Length, 1, AudioSettings.outputSampleRate, false);
                    audioClip.SetData(AudioSamples, 0);
                }

                return audioClip;
            }
        }
        private AudioClip audioClip;

        [Preserve]
        [JsonIgnore]
        public string Transcript { get; private set; }

        [Preserve]
        public override string ToString() => Transcript ?? string.Empty;

        [Preserve]
        public static implicit operator AudioClip(AudioOutput audioOutput) => audioOutput?.AudioClip;

        [Preserve]
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
