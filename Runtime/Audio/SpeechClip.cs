// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Audio
{
    [Preserve]
    public sealed class SpeechClip
    {
        [Preserve]
        internal SpeechClip(string name, string cachePath, ReadOnlyMemory<byte> audioData, int sampleRate = 24000)
        {
            Name = name;
            CachePath = cachePath;
            AudioData = audioData;
            SampleRate = sampleRate;
        }

        [Preserve]
        public string Name { get; }

        [Preserve]
        public string CachePath { get; }

        [Preserve]
        public ReadOnlyMemory<byte> AudioData { get; }

        [Preserve]
        public float[] AudioSamples
            => PCMEncoder.Decode(AudioData.ToArray(), inputSampleRate: SampleRate, outputSampleRate: AudioSettings.outputSampleRate);

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
    }
}
