// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
        public Role Role { get; internal set; }

        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<Content> Content { get; }

        /// <summary>
        /// Formats all of the <see cref="Content"/> items into a single string,
        /// putting each item on a new line.
        /// </summary>
        /// <returns><see cref="string"/> of all <see cref="Content"/>.</returns>
        public string PrintContent()
            => Content == null
                ? string.Empty
                : string.Join("\n", Content.Select(c => c?.ToString()));
    }
}
