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
            => PCMEncoder.Resample(PCMEncoder.Decode(AudioData.ToArray()), SampleRate, 44100);

        [Preserve]
        public int SampleRate { get; }

        [Preserve]
        public AudioClip AudioClip
        {
            get
            {
                var samples = AudioSamples;
                var clip = AudioClip.Create(Name, samples.Length, 1, 44100, false);
                clip.SetData(samples, 0);
                return clip;
            }
        }

        [Preserve]
        public static implicit operator AudioClip(SpeechClip clip) => clip?.AudioClip;

        [Preserve]
        public static implicit operator string(SpeechClip clip) => clip?.CachePath;
    }
}
