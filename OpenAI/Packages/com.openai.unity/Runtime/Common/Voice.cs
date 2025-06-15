// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    [Serializable]
    public class Voice
    {
        [Preserve]
        public Voice(string id) { Id = id; }

        [SerializeField]
        private string id;

        [Preserve]
        public string Id
        {
            get => id;
            private set => id = value;
        }

        [Preserve]
        public override string ToString() => Id;

        [Preserve]
        public static implicit operator string(Voice voice) => voice?.ToString();

        [Preserve]
        public static implicit operator Voice(string id) => new(id);

        [Preserve]
        public static readonly Voice Alloy = new("alloy");

        [Preserve]
        public static readonly Voice Ash = new("ash");

        [Preserve]
        public static readonly Voice Ballad = new("ballad");

        [Preserve]
        public static readonly Voice Coral = new("coral");

        [Preserve]
        public static readonly Voice Echo = new("echo");

        [Preserve]
        public static readonly Voice Fable = new("fable");

        [Preserve]
        public static readonly Voice Onyx = new("onyx");

        [Preserve]
        public static readonly Voice Nova = new("nova");

        [Preserve]
        public static readonly Voice Sage = new("sage");

        [Preserve]
        public static readonly Voice Shimmer = new("shimmer");

        [Preserve]
        public static readonly Voice Verse = new("verse");

        public static readonly string[] All =
        {
            Alloy,
            Ash,
            Ballad,
            Coral,
            Echo,
            Fable,
            Onyx,
            Nova,
            Sage,
            Shimmer,
            Verse
        };

#pragma warning disable CS0618 // Type or member is obsolete
        public static implicit operator Voice(Audio.SpeechVoice voice)
        {
            return voice switch
            {
                Audio.SpeechVoice.Alloy => Alloy,
                Audio.SpeechVoice.Echo => Echo,
                Audio.SpeechVoice.Fable => Fable,
                Audio.SpeechVoice.Onyx => Onyx,
                Audio.SpeechVoice.Nova => Nova,
                Audio.SpeechVoice.Shimmer => Shimmer,
                _ => null
            };
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
