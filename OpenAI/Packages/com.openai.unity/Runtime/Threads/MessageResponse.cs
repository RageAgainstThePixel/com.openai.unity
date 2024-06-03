// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    /// <summary>
    /// A message created by an Assistant or a user.
    /// Messages can include text, images, and other files.
    /// Messages stored as a list on the Thread.
    /// </summary>
    [Preserve]
    public sealed class MessageResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal MessageResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("thread_id")] string threadId,
            [JsonProperty("status")] MessageStatus status,
            [JsonProperty("incomplete_details")] IncompleteDetails incompleteDetails,
            [JsonProperty("completed_at")] int? completedAtUnixTimeSeconds,
            [JsonProperty("incomplete_at")] int? incompleteAtUnixTimeSeconds,
            [JsonProperty("role")] Role role,
            [JsonProperty("content")] IReadOnlyList<Content> content,
            [JsonProperty("assistant_id")] string assistantId,
            [JsonProperty("run_id")] string runId,
            [JsonProperty("Attachments")] IReadOnlyList<Attachment> attachments,
            [JsonProperty("metadata")] Dictionary<string, string> metadata)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            ThreadId = threadId;
            Status = status;
            IncompleteDetails = incompleteDetails;
            CompletedAtUnixTimeSeconds = completedAtUnixTimeSeconds;
            IncompleteAtUnixTimeSeconds = incompleteAtUnixTimeSeconds;
            Role = role;
            Content = content;
            AssistantId = assistantId;
            RunId = runId;
            Attachments = attachments;
            Metadata = metadata;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always message.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The thread ID that this message belongs to.
        /// </summary>
        [Preserve]
        [JsonProperty("thread_id")]
        public string ThreadId { get; }

        /// <summary>
        /// The status of the message, which can be either 'in_progress', 'incomplete', or 'completed'.
        /// </summary>
        [Preserve]
        [JsonProperty("status")]
        public MessageStatus Status { get; private set; }

        /// <summary>
        /// On an incomplete message, details about why the message is incomplete.
        /// </summary>
        [Preserve]
        [JsonProperty("incomplete_details")]
        public IncompleteDetails IncompleteDetails { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was completed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed_at")]
        public int? CompletedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? CompletedAt
            => CompletedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CompletedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was marked as incomplete.
        /// </summary>
        [Preserve]
        [JsonProperty("incomplete_at")]
        public int? IncompleteAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? IncompleteAt
            => IncompleteAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(IncompleteAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The entity that produced the message. One of user or assistant.
        /// </summary>
        [Preserve]
        [JsonProperty("role")]
        public Role Role { get; }

        /// <summary>
        /// The content of the message in array of text and/or images.
        /// </summary>
        [Preserve]
        [JsonProperty("content")]
        public IReadOnlyList<Content> Content { get; }

        /// <summary>
        /// If applicable, the ID of the assistant that authored this message.
        /// </summary>
        [Preserve]
        [JsonProperty("assistant_id")]
        public string AssistantId { get; }

        /// <summary>
        /// If applicable, the ID of the run associated with the authoring of this message.
        /// </summary>
        [Preserve]
        [JsonProperty("run_id")]
        public string RunId { get; }

        /// <summary>
        /// A list of file IDs that the assistant should use.
        /// Useful for tools like 'retrieval' and 'code_interpreter' that can access files.
        /// A maximum of 10 files can be attached to a message.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use Attachments instead.")]
        public IReadOnlyList<string> FileIds => Attachments?.Select(attachment => attachment.FileId).ToList();

        /// <summary>
        /// A list of files attached to the message, and the tools they were added to.
        /// </summary>
        [Preserve]
        [JsonProperty("Attachments")]
        public IReadOnlyList<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        [Preserve]
        public static implicit operator string(MessageResponse message) => message?.ToString();

        [Preserve]
        public static implicit operator Message(MessageResponse response)
            => new(response.Content, response.Role, response.Attachments, response.Metadata);

        [Preserve]
        public override string ToString() => Id;

        /// <summary>
        /// Formats all of the <see cref="Content"/> items into a single string,
        /// putting each item on a new line.
        /// </summary>
        /// <returns><see cref="string"/> of all <see cref="Content"/>.</returns>
        [Preserve]
        public string PrintContent()
            => string.Join("\n", Content.Select(content => content?.ToString()));
    }
}
