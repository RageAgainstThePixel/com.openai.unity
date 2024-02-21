// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    [Obsolete("use EventResponse")]
    public sealed class Event : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public Event(
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTime,
            [JsonProperty("level")] string level,
            [JsonProperty("message")] string message)
        {
            Object = @object;
            CreatedAtUnixTime = createdAtUnixTime;
            Level = level;
            Message = message;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [Preserve]
        [JsonProperty("level")]
        public string Level { get; }

        [Preserve]
        [JsonProperty("message")]
        public string Message { get; }

        public static implicit operator EventResponse(Event @event) => new(@event);
    }
}
