// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool call to a computer use tool.
    /// </summary>
    [Preserve]
    public sealed class ComputerToolCall : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ComputerToolCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("call_id")] string callId,
            [JsonProperty("action")] IComputerAction action,
            [JsonProperty("pending_safety_checks")] IReadOnlyList<ComputerToolCallSafetyCheck> pendingSafetyChecks,
            [JsonProperty("output")] ComputerScreenShot computerScreenShot)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            CallId = callId;
            Action = action;
            PendingSafetyChecks = pendingSafetyChecks;
            ComputerScreenShot = computerScreenShot;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        /// <summary>
        /// An identifier used when responding to the tool call with output.
        /// </summary>
        [Preserve]
        [JsonProperty("call_id")]
        public string CallId { get; }

        /// <summary>
        /// An action for the computer use tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("action")]
        public IComputerAction Action { get; }

        /// <summary>
        /// The pending safety checks for the computer call.
        /// </summary>
        [Preserve]
        [JsonProperty("pending_safety_checks", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<ComputerToolCallSafetyCheck> PendingSafetyChecks { get; }

        /// <summary>
        /// A computer screenshot image used with the computer use tool.
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public ComputerScreenShot ComputerScreenShot { get; }
    }
}
