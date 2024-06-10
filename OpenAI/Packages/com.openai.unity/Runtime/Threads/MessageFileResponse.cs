// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    [Obsolete("Removed. Use Assistant.ToolResources instead.")]
    public sealed class MessageFileResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public MessageFileResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("message_id")] string messageId)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            MessageId = messageId;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always thread.message.file.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message file was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The ID of the message that the File is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("message_id")]
        public string MessageId { get; }

        [Preserve]
        public static implicit operator string(MessageFileResponse response) => response?.ToString();

        [Preserve]
        public override string ToString() => Id;
    }
}
