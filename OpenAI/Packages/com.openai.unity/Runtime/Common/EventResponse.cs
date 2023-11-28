// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    public sealed class EventResponse : BaseResponse
    {
        public EventResponse() { }

#pragma warning disable CS0618 // Type or member is obsolete
        internal EventResponse(Event @event)
        {
            Object = @event.Object;
            CreatedAtUnixTimeSeconds = @event.CreatedAtUnixTime;
            Level = @event.Level;
            Message = @event.Message;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        [Preserve]
        [JsonProperty("level")]
        public string Level { get; private set; }

        [Preserve]
        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}
