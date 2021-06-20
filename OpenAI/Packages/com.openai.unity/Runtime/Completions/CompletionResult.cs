// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenAI
{
    /// <summary>
    /// Represents a result from calling the <see cref="CompletionEndpoint"/>.
    /// </summary>
    public sealed class CompletionResult : BaseResponse
    {
        /// <summary>
        /// The identifier of the result, which may be used during troubleshooting
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [JsonProperty("created")]
        public int CreatedUnixTime { get; set; }

        /// The time when the result was generated
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        private Engine engine;

        /// <summary>
        /// Which model was used to generate this result. Be sure to check <see cref="OpenAI.Engine.ModelRevision"/> for the specific revision number.
        /// </summary>
        [JsonIgnore]
        public Engine Engine
        {
            get => engine ?? new Engine(Model);
            set => engine = value;
        }

        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// The completions returned by the API.  Depending on your request, there may be 1 or many choices.
        /// </summary>
        [JsonProperty("choices")]
        public List<Choice> Completions { get; set; }

        /// <summary>
        /// Gets the text of the first completion, representing the main result
        /// </summary>
        public override string ToString()
        {
            return Completions != null && Completions.Count > 0
                ? Completions[0].ToString()
                : $"CompletionResult {Id} has no valid output";
        }
    }
}
