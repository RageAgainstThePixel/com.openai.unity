// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Completions
{
    /// <summary>
    /// Represents a result from calling the <see cref="CompletionsEndpoint"/>.
    /// </summary>
    [Preserve]
    public sealed class CompletionResult : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public CompletionResult(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int createdUnixTime,
            [JsonProperty("model")] string model,
            [JsonProperty("choices")] IReadOnlyList<Choice> completions)
        {
            Id = id;
            Object = @object;
            CreatedUnixTime = createdUnixTime;
            Model = model;
            Completions = completions;
        }

        /// <summary>
        /// The identifier of the result, which may be used during troubleshooting
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [Preserve]
        [JsonProperty("created")]
        public int CreatedUnixTime { get; }

        /// <summary>
        /// The time when the result was generated
        /// </summary>
        [Preserve]
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The completions returned by the API.  Depending on your request, there may be 1 or many choices.
        /// </summary>
        [Preserve]
        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Completions { get; }

        [Preserve]
        [JsonIgnore]
        public Choice FirstChoice => Completions?.FirstOrDefault(choice => choice.Index == 0);

        public override string ToString() => FirstChoice?.ToString() ?? string.Empty;

        public static implicit operator string(CompletionResult response) => response.ToString();
    }
}
