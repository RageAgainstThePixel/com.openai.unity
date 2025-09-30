// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class Conversation
    {
        [JsonConstructor]
        internal Conversation(
            [JsonProperty("created_at")] long createdAtUnitTimeSeconds,
            [JsonProperty("id")] string id,
            [JsonProperty("metadata")] Dictionary<string, string> metaData = null,
            [JsonProperty("object")] string @object = null)
        {
            CreatedAtUnitTimeSeconds = createdAtUnitTimeSeconds;
            Id = id;
            MetaData = metaData;
            Object = @object;
        }

        /// <summary>
        /// The time at which the conversation was created, measured in seconds since the Unix epoch.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public long CreatedAtUnitTimeSeconds { get; }

        /// <summary>
        /// The datetime at which the conversation was created.
        /// </summary>
        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt
            => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnitTimeSeconds).UtcDateTime;

        /// <summary>
        /// The unique ID of the conversation.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> MetaData { get; }

        /// <summary>
        /// The object type, which is always conversation.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        public override string ToString() => Id;

        [Preserve]
        public static implicit operator string(Conversation conversation) => conversation?.Id;
    }
}
