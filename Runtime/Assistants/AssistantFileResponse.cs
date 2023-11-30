// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Assistants
{
    /// <summary>
    /// File attached to an assistant.
    /// </summary>
    [Preserve]
    public sealed class AssistantFileResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public AssistantFileResponse(
            string id,
            string @object,
            int createdAtUnixTimeSeconds,
            string assistantId)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            AssistantId = assistantId;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, which is always assistant.file.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the assistant file was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The assistant ID that the file is attached to.
        /// </summary>
        [Preserve]
        [JsonProperty("assistant_id")]
        public string AssistantId { get; private set; }

        [Preserve]
        public static implicit operator string(AssistantFileResponse file) => file?.ToString();

        [Preserve]
        public override string ToString() => Id;
    }
}
