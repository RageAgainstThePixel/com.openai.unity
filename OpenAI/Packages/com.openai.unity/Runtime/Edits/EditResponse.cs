// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenAI.Edits
{
    public sealed class EditResponse : BaseResponse
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [JsonProperty("created")]
        public int CreatedUnixTime { get; set; }

        /// The time when the result was generated
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [JsonProperty("choices")]
        public List<Choice> Choices { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        /// <summary>
        /// Gets the text of the first edit, representing the main result
        /// </summary>
        public override string ToString()
        {
            return Choices is { Count: > 0 }
                ? Choices[0]
                : "Edit result has no valid output";
        }
    }
}
