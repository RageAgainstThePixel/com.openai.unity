// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class FinishDetails
    {
        [Preserve]
        public FinishDetails() { }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        [Preserve]
        public override string ToString() => Type;

        [Preserve]
        public static implicit operator string(FinishDetails details) => details?.ToString();
    }
}
