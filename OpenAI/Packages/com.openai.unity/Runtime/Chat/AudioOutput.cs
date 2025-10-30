// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;
using Utilities.Extensions;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class AudioOutput : IDisposable
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

        ~AudioOutput() => Dispose(false);

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
        public NativeArray<byte> AudioData
        {
            get
            {
                if (!audioData.HasValue ||
                    audioData.Value.Length == 0)
                {
                    audioData = NativeArrayExtensions.FromBase64String(Data, Allocator.Persistent);
                }

                return audioData.Value;
            }
        }
        private NativeArray<byte>? audioData;

        [Preserve]
        [JsonIgnore]
        public string Data { get; private set; }

        [Preserve]
        [JsonIgnore]
        public NativeArray<float> AudioSamples
            => audioSamples ??= PCMEncoder.Decode(AudioData, inputSampleRate: 24000, outputSampleRate: AudioSettings.outputSampleRate);
        private NativeArray<float>? audioSamples;

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

        [Preserve]
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                audioSamples?.Dispose();
                AudioData.Dispose();
            }
        }

        [Preserve]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
