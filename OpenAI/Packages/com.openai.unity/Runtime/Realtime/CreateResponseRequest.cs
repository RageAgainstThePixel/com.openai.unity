// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
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
    public sealed class CreateResponseRequest : BaseRealtimeEvent, IClientEvent
    {
        [Preserve]
        public CreateResponseRequest()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Inference configuration <see cref="Realtime.Options"/> to override the <see cref="RealtimeSession.Options"/> for this response only.</param>
        [Obsolete]
        [Preserve]
        public CreateResponseRequest(Options options)
        {
            Options = options;
        }

        public CreateResponseRequest(RealtimeResponseCreateParams options)
        {
            Options = options;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; } = "response.create";

        [Preserve]
        [JsonProperty("response")]
        public RealtimeResponseCreateParams Options { get; }
    }
}
