// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class MessageAbstract
    {
        [Preserve]
        [JsonConstructor]
        public MessageAbstract(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object)
        {
            Id = id;
            Object = @object;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }
    }

    [Preserve]
    public sealed class MessageDelta
    {
        [Preserve]
        [JsonConstructor]
        public MessageDelta(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("delta")] Delta delta)
        {
            Id = id;
            Object = @object;
            Delta = delta;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        [JsonProperty("delta")]
        public Delta Delta { get; private set; }
    }

    [System.Serializable]
    public sealed class Delta
    {
        [Preserve]
        [JsonProperty("step_details")]
        public StepDetails step_details;
        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<Content> content;
        [Preserve]
        public string PrintContent() => string.Join("\n", content.Select(content => content?.ToString()));

    }
}