// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Edits
{
    [Obsolete]
    [Preserve]
    public sealed class EditResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public EditResponse(
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int createdUnixTime,
            [JsonProperty("choices")] IReadOnlyList<Choice> choices,
            [JsonProperty("usage")] Usage usage)
        {
            Object = @object;
            CreatedUnixTime = createdUnixTime;
            Choices = choices;
            Usage = usage;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The time when the result was generated in unix epoch format
        /// </summary>
        [Preserve]
        [JsonProperty("created")]
        public int CreatedUnixTime { get; }

        /// The time when the result was generated
        [Preserve]
        [JsonIgnore]
        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [Preserve]
        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices { get; }

        [Preserve]
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
