// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseFunctionCallArguments : BaseRealtimeEvent, IServerEvent, IRealtimeEventStream
    {
        [Preserve]
        [JsonConstructor]
        internal ResponseFunctionCallArguments(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("response_id")] string responseId,
            [JsonProperty("item_id")] string itemId,
            [JsonProperty("output_index")] string outputIndex,
            [JsonProperty("call_id")] string callId,
            [JsonProperty("delta")] string delta,
            [JsonProperty("arguments")] string arguments)
        {
            EventId = eventId;
            Type = type;
            ResponseId = responseId;
            ItemId = itemId;
            OutputIndex = outputIndex;
            CallId = callId;
            Delta = delta;
            Arguments = arguments;
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
        /// The ID of the response.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public string OutputIndex { get; }

        /// <summary>
        /// The ID of the function call.
        /// </summary>
        [Preserve]
        [JsonProperty("call_id")]
        public string CallId { get; }

        /// <summary>
        /// The arguments delta as a JSON string.
        /// </summary>
        [Preserve]
        [JsonProperty("delta")]
        public string Delta { get; }

        /// <summary>
        /// The final arguments as a JSON string.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string Arguments { get; }

        [Preserve]
        [JsonIgnore]
        public bool IsDelta => Type.EndsWith("delta");

        [Preserve]
        [JsonIgnore]
        public bool IsDone => Type.EndsWith("done");
    }
}
