// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public class Voice
    {
        [Preserve]
        public Voice(string id) { Id = id; }

        [Preserve]
        public string Id { get; }

        [Preserve]
        public override string ToString() => Id;

        [Preserve]
        public static implicit operator string(Voice voice) => voice?.ToString();

        [Preserve]
        public static implicit operator Voice(string id) => new(id);

        public static readonly Voice Alloy = new("alloy");
        public static readonly Voice Ash = new("ash");
        public static readonly Voice Ballad = new("ballad");
        public static readonly Voice Coral = new("coral");
        public static readonly Voice Echo = new("echo");
        public static readonly Voice Fable = new("fable");
        public static readonly Voice Onyx = new("onyx");
        public static readonly Voice Nova = new("nova");
        public static readonly Voice Sage = new("sage");
        public static readonly Voice Shimmer = new("shimmer");
        public static readonly Voice Verse = new("verse");

#pragma warning disable CS0618 // Type or member is obsolete
        public static implicit operator Voice(SpeechVoice voice)
        {
            return voice switch
            {
                SpeechVoice.Alloy => Alloy,
                SpeechVoice.Echo => Echo,
                SpeechVoice.Fable => Fable,
                SpeechVoice.Onyx => Onyx,
                SpeechVoice.Nova => Nova,
                SpeechVoice.Shimmer => Shimmer,
                _ => null
            };
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
