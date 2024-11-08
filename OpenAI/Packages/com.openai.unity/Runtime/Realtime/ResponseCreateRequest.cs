// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    /// <summary>
    /// This event instructs the server to create a Response, which means triggering model inference.
    /// When in Server VAD mode, the server will create Responses automatically.
    /// A Response will include at least one Item, and may have two, in which case the second will be a function call.
    /// These Items will be appended to the conversation history. The server will respond with a response.created event,
    /// events for Items and content created, and finally a response.done event to indicate the Response is complete.
    /// The response.create event includes inference configuration like instructions, and temperature.
    /// These fields will override the Session's configuration for this Response only.
    /// </summary>
    [Preserve]
    public sealed class ResponseCreateRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public ResponseCreateRequest(RealtimeResponseResource response)
        {
            Response = response;
        }

        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "response.create";

        [Preserve]
        [JsonProperty("response")]
        public RealtimeResponseResource Response { get; }
    }
}
