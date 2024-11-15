// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class InputAudioBufferCommittedResponse : BaseRealtimeEvent, IServerEvent
    {
        [Preserve]
        [JsonConstructor]
        internal InputAudioBufferCommittedResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("previous_item_id")] string previousItemId,
            [JsonProperty("item_id")] string itemId)
        {
            EventId = eventId;
            Type = type;
            PreviousItemId = previousItemId;
            ItemId = itemId;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; }

        /// <summary>
        /// The ID of the preceding item after which the new item will be inserted.
        /// </summary>
        [Preserve]
        [JsonProperty("previous_item_id")]
        public string PreviousItemId { get; }

        /// <summary>
        /// The ID of the user message item that will be created.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }
    }
}
