// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI
{
    public sealed class Event
    {
        [JsonConstructor]
        public Event(
            string @object,
            int createdAtUnixTime,
            string level,
            string message
        )
        {
            Object = @object;
            CreatedAtUnixTime = createdAtUnixTime;
            Level = level;
            Message = message;
        }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("created_at")]
        public int CreatedAtUnixTime { get; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [JsonProperty("level")]
        public string Level { get; }

        [JsonProperty("message")]
        public string Message { get; }
    }
}
