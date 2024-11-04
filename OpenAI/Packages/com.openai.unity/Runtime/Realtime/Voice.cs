// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace OpenAI.Realtime
{
    public sealed class Voice
    {
        public Voice(string id) { Id = id; }

        public string Id { get; }

        public override string ToString() => Id;

        public static implicit operator string(Voice voice) => voice?.ToString();

        public static readonly Voice Alloy = new("alloy");
        public static readonly Voice Ash = new("ash");
        public static readonly Voice Ballad = new("ballad");
        public static readonly Voice Coral = new("coral");
        public static readonly Voice Echo = new("echo");
        public static readonly Voice Sage = new("sage");
        public static readonly Voice Shimmer = new("shimmer");
        public static readonly Voice Verse = new("verse");
    }
}
