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
            audioData = audioClip.EncodeToPCM(allocator: Allocator.Persistent);
        }

        [Preserve]
        internal SpeechClip(string name, string cachePath, byte[] audioData, int sampleRate = 24000)
        {
            Name = name;
            CachePath = cachePath;
            this.audioData = new NativeArray<byte>(audioData, Allocator.Persistent);
            SampleRate = sampleRate;
        }

        ~SpeechClip() => Dispose();

        [Preserve]
        public string Name { get; }

        [Preserve]
        public string CachePath { get; }

        [Preserve]
        public NativeArray<byte> AudioData
            => audioData ??= new NativeArray<byte>(0, Allocator.Persistent);
        private NativeArray<byte>? audioData;

        [Preserve]
        public NativeArray<float> AudioSamples
        {
            get
            {
                if (!audioData.HasValue)
                {
                    return new NativeArray<float>(0, Allocator.Persistent);
                }

                audioSamples ??= PCMEncoder.Decode(
                    pcmData: AudioData,
                    inputSampleRate: SampleRate,
                    outputSampleRate: AudioSettings.outputSampleRate,
                    allocator: Allocator.Persistent);
                return audioSamples.Value;
            }
        }

        private NativeArray<float>? audioSamples;

        [Preserve]
        public int SampleRate { get; }

        [Preserve]
        public AudioClip AudioClip
        {
            get
            {
                if (audioClip == null && (audioSamples.HasValue || audioData.HasValue))
                {
                    audioClip = AudioClip.Create(Name, AudioSamples.Length, 1, AudioSettings.outputSampleRate, false);
#if UNITY_6000_0_OR_NEWER
                    audioClip.SetData(AudioSamples, 0);
#else
                    audioClip.SetData(AudioSamples.ToArray(), 0);
#endif
                }

                return audioClip;
            }
        }
        private AudioClip audioClip;

        [Preserve]
        public float Length
        {
            get
            {
                if (audioClip != null)
                {
                    return audioClip.length;
                }

                if (!audioSamples.HasValue || !audioData.HasValue)
                {
                    return 0;
                }

                return AudioSamples.Length / (float)AudioSettings.outputSampleRate;
            }
        }

        [Preserve]
        public static implicit operator AudioClip(SpeechClip clip) => clip?.AudioClip;

        [Preserve]
        public static implicit operator string(SpeechClip clip) => clip?.CachePath;

        [Preserve]
        public void Dispose()
        {
            audioSamples?.Dispose();
            audioSamples = null;
            audioData?.Dispose();
            audioData = null;
        }
    }
}
