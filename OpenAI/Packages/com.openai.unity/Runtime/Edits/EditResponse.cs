// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenAI.Edits
{
    [Obsolete]
    public sealed class EditResponse : BaseResponse
    {
        [JsonConstructor]
        public EditResponse(
            string @object,
            int createdUnixTime,
            IReadOnlyList<Choice> choices,
            Usage usage)
        {
            Object = @object;
            CreatedUnixTime = createdUnixTime;
            Choices = choices;
            Usage = usage;
        }

        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [JsonProperty("created")]
        public int CreatedUnixTime { get; }

        /// The time when the result was generated
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices { get; }

        [JsonProperty("usage")]
        public Usage Usage { get; }

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
