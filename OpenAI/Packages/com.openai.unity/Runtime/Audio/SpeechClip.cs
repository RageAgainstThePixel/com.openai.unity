// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Audio
{
    [Preserve]
    public sealed class SpeechClip : IDisposable
    {
        [Preserve]
        internal SpeechClip(string name, string cachePath, AudioClip audioClip)
        {
            Name = name;
            CachePath = cachePath;
            this.audioClip = audioClip;
            SampleRate = audioClip.frequency;
            AudioData = audioClip.EncodeToPCM(allocator: Allocator.Persistent);
        }

        [Preserve]
        internal SpeechClip(string name, string cachePath, byte[] audioData, int sampleRate = 24000)
        {
            Name = name;
            CachePath = cachePath;
            AudioData = new NativeArray<byte>(audioData, Allocator.Persistent);
            SampleRate = sampleRate;
        }

        ~SpeechClip() => Dispose(false);

        [Preserve]
        public string Name { get; }

        [Preserve]
        public string CachePath { get; }

        [Preserve]
        public NativeArray<byte> AudioData { get; }

        [Preserve]
        public NativeArray<float> AudioSamples
            => audioSamples ??= PCMEncoder.Decode(AudioData, inputSampleRate: SampleRate, outputSampleRate: AudioSettings.outputSampleRate);
        private NativeArray<float>? audioSamples;

        [Preserve]
        public int SampleRate { get; }

        [Preserve]
        public AudioClip AudioClip
        {
            get
            {
                if (audioClip == null)
                {
                    audioClip = AudioClip.Create(Name, AudioSamples.Length, 1, AudioSettings.outputSampleRate, false);
                    audioClip.SetData(AudioSamples, 0);
                }

                return audioClip;
            }
        }
        private AudioClip audioClip;

        [Preserve]
        public float Length => AudioSamples.Length / (float)AudioSettings.outputSampleRate;

        [Preserve]
        public static implicit operator AudioClip(SpeechClip clip) => clip?.AudioClip;

        [Preserve]
        public static implicit operator string(SpeechClip clip) => clip?.CachePath;

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
