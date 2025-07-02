// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ExpiresAfter
    {
        [Preserve]
        public ExpiresAfter(int seconds = 600)
        {
            Seconds = seconds;
        }

        [Preserve]
        [JsonConstructor]
        internal ExpiresAfter(string anchor, int seconds)
        {
            Anchor = anchor;
            Seconds = seconds;
        }

        [Preserve]
        [JsonProperty("anchor")]
        public string Anchor { get; } = "created_at";

        [Preserve]
        [JsonProperty("seconds")]
        public int Seconds { get; }

        [Preserve]
        public static implicit operator ExpiresAfter(int seconds)
            => new(seconds);
    }
}
