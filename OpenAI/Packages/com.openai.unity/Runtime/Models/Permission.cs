// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Models
{
    public sealed class Permission
    {
        [JsonConstructor]
        public Permission(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int createdAtUnixTime,
            [JsonProperty("allow_create_engine")] bool allowCreateEngine,
            [JsonProperty("allow_sampling")] bool allowSampling,
            [JsonProperty("allow_logprobs")] bool allowLogprobs,
            [JsonProperty("allow_search_indices")] bool allowSearchIndices,
            [JsonProperty("allow_view")] bool allowView,
            [JsonProperty("allow_fine_tuning")] bool allowFineTuning,
            [JsonProperty("organization")] string organization,
            [JsonProperty("group")] object group,
            [JsonProperty("is_blocking")] bool isBlocking)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTime = createdAtUnixTime;
            AllowCreateEngine = allowCreateEngine;
            AllowSampling = allowSampling;
            AllowLogprobs = allowLogprobs;
            AllowSearchIndices = allowSearchIndices;
            AllowView = allowView;
            AllowFineTuning = allowFineTuning;
            Organization = organization;
            Group = group;
            IsBlocking = isBlocking;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("created")]
        public int CreatedAtUnixTime { get; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [JsonProperty("allow_create_engine")]
        public bool AllowCreateEngine { get; }

        [JsonProperty("allow_sampling")]
        public bool AllowSampling { get; }

        [JsonProperty("allow_logprobs")]
        public bool AllowLogprobs { get; }

        [JsonProperty("allow_search_indices")]
        public bool AllowSearchIndices { get; }

        [JsonProperty("allow_view")]
        public bool AllowView { get; }

        [JsonProperty("allow_fine_tuning")]
        public bool AllowFineTuning { get; }

        [JsonProperty("organization")]
        public string Organization { get; }

        [JsonProperty("group")]
        public object Group { get; }

        [JsonProperty("is_blocking")]
        public bool IsBlocking { get; }
    }
}
