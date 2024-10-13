// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseFunctionCallArguments : BaseRealtimeEventResponse, IRealtimeEvent
    {
        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public string EventId { get; private set; }

        /// <summary>
        /// "response.function_call_arguments.delta" or "response.function_call_arguments.done"
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        /// <summary>
        /// The ID of the response.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; private set; }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; private set; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public string OutputIndex { get; private set; }

        /// <summary>
        /// The ID of the function call.
        /// </summary>
        [Preserve]
        [JsonProperty("call_id")]
        public string CallId { get; private set; }

        /// <summary>
        /// The arguments delta as a JSON string.
        /// </summary>
        [Preserve]
        [JsonProperty("delta")]
        public string Delta { get; private set; }

        /// <summary>
        /// The final arguments as a JSON string.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string Arguments { get; private set; }
    }
}
