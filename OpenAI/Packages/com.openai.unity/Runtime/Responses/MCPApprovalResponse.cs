// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class MCPApprovalResponse : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal MCPApprovalResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("approval_request_id")] string approvalRequestId,
            [JsonProperty("approve")] bool approve,
            [JsonProperty("reason")] string reason)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            ApprovalRequestId = approvalRequestId;
            Approve = approve;
            Reason = reason;
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

        [Preserve]
        [JsonProperty("approval_request_id")]
        public string ApprovalRequestId { get; }

        [Preserve]
        [JsonProperty("approve")]
        public bool Approve { get; }

        /// <summary>
        /// Optional reason for the decision.
        /// </summary>
        [Preserve]
        [JsonProperty("reason", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Reason { get; }
    }
}
