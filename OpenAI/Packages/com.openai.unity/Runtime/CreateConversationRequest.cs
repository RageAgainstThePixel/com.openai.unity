// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class CreateConversationRequest
    {
        public CreateConversationRequest(IResponseItem item, IReadOnlyDictionary<string, string> metadata = null)
            : this(new[] { item }, metadata)
        {
        }

        public CreateConversationRequest(IEnumerable<IResponseItem> items = null, IReadOnlyDictionary<string, string> metadata = null)
        {
            Items = items;
            Metadata = metadata;
        }

        [JsonConstructor]
        internal CreateConversationRequest(List<IResponseItem> items, Dictionary<string, string> metadata)
        {
            Items = items;
            Metadata = metadata;
        }

        /// <summary>
        /// Initial items to include in the conversation context. You may add up to 20 items at a time.
        /// </summary>
        [Preserve]
        [JsonProperty("items", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<IResponseItem> Items { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format,
        /// and querying for objects via API or the dashboard. Keys are strings with a maximum length of 64 characters.
        /// Values are strings with a maximum length of 512 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> Metadata { get; set; }
    }
}
