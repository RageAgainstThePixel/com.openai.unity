// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class MessageDelta
    {
        [Preserve]
        [JsonConstructor]
        internal MessageDelta(
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] IReadOnlyList<Content> content)
        {
            Role = role;
            Content = content;
        }

        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; private set; }

        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<Content> Content { get; }
    };
}
