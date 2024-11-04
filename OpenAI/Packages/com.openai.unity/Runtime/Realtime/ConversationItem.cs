﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ConversationItem
    {
        /// <summary>
        /// The unique ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, must be "realtime.item".
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The type of the item ("message", "function_call", "function_call_output").
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public ConversationItemType Type { get; private set; }

        /// <summary>
        /// The status of the item ("completed", "in_progress", "incomplete").
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public RealtimeResponseStatus Status { get; private set; }

        /// <summary>
        /// The role associated with the item ("user", "assistant", "system").
        /// </summary>
        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; private set; }

        /// <summary>
        /// The content of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<RealtimeContent> Content { get; private set; }

        /// <summary>
        /// The ID of the function call (for "function_call" items).
        /// </summary>
        [Preserve]
        [JsonProperty("call_id")]
        public string FunctionCallId { get; private set; }

        /// <summary>
        /// The name of the function being called.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string FunctionName { get; private set; }

        /// <summary>
        /// The arguments of the function call.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public string FunctionArguments { get; private set; }

        /// <summary>
        /// The output of the function call (for "function_call_output" items).
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string FunctionOutput { get; private set; }
    }
}
