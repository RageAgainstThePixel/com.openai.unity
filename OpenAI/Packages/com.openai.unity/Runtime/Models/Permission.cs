// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Models
{
    [Preserve]
    public sealed class Permission
    {
        [Preserve]
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

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("created")]
        public int CreatedAtUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [Preserve]
        [JsonProperty("allow_create_engine")]
        public bool AllowCreateEngine { get; }

        [Preserve]
        [JsonProperty("allow_sampling")]
        public bool AllowSampling { get; }

        [Preserve]
        [JsonProperty("allow_logprobs")]
        public bool AllowLogprobs { get; }

        [Preserve]
        [JsonProperty("allow_search_indices")]
        public bool AllowSearchIndices { get; }

        [Preserve]
        [JsonProperty("allow_view")]
        public bool AllowView { get; }

        [Preserve]
        [JsonProperty("allow_fine_tuning")]
        public bool AllowFineTuning { get; }

        [Preserve]
        [JsonProperty("organization")]
        public string Organization { get; }

        [Preserve]
        [JsonProperty("group")]
        public object Group { get; }

        [Preserve]
        [JsonProperty("is_blocking")]
        public bool IsBlocking { get; }
    }
}
