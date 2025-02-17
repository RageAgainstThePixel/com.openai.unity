// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// Send this event to cancel an in-progress response.
    /// The server will respond with a `response.cancelled` event or an error if there is no response to cancel.
    /// </summary>
    [Preserve]
    public sealed class ResponseCancelRequest : BaseRealtimeEvent, IClientEvent
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ResponseCancelRequest() { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseId">
        /// A specific response ID to cancel - if not provided, will cancel an in-progress response in the default conversation.
        /// </param>
        public ResponseCancelRequest(string responseId)
        {
            ResponseId = responseId;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "response.cancel";

        /// <summary>
        /// A specific response ID to cancel - if not provided, will cancel an in-progress response in the default conversation.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; }
    }
}
