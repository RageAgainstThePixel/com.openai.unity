// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class EventList
    {
        [Preserve]
        [JsonConstructor]
        public EventList(
            [JsonProperty("object")] string @object,
            [JsonProperty("data")] IReadOnlyList<Event> events,
            [JsonProperty("has_more")] bool hasMore)
        {
            Object = @object;
            Events = events;
            HasMore = hasMore;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<Event> Events { get; }

        [Preserve]
        [JsonProperty("has_more")]
        public bool HasMore { get; }
    }
}
