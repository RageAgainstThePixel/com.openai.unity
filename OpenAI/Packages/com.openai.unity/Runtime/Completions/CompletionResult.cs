// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Completions
{
    /// <summary>
    /// Represents a result from calling the <see cref="CompletionsEndpoint"/>.
    /// </summary>
    public sealed class CompletionResult : BaseResponse
    {
        [JsonConstructor]
        public CompletionResult(
            string id,
            string @object,
            int createdUnixTime,
            string model,
            IReadOnlyList<Choice> completions)
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
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [JsonProperty("created")]
        public int CreatedUnixTime { get; }

        /// <summary>
        /// The time when the result was generated
        /// </summary>
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The completions returned by the API.  Depending on your request, there may be 1 or many choices.
        /// </summary>
        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Completions { get; }

        /// <summary>
        /// Gets the text of the first completion, representing the main result
        /// </summary>
        public override string ToString() => Completions.Count switch
        {
            0 => $"CompletionResult {Id} has no valid output",
            1 => Completions[0].ToString(),
            _ => Completions.FirstOrDefault(choice => choice.Index == 0)?.ToString()
        };
    }
}
