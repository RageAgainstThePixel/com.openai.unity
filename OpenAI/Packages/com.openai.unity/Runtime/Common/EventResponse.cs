// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    public sealed class EventResponse : BaseResponse
    {
        public EventResponse() { }

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
