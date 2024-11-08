// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeEventError : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal RealtimeEventError(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("error")] Error error)
        {
            EventId = eventId;
            Type = type;
            Error = error;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; }

        [Preserve]
        [JsonProperty("error")]
        public Error Error { get; }

        [Preserve]
        public override string ToString()
            => Error.ToString();

        [Preserve]
        public static implicit operator Exception(RealtimeEventError error)
            => error.Error?.Exception ?? new Exception(error.ToString());
    }
}
