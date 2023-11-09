// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class ChatResponse : BaseResponse
    {
        [Preserve]
        internal ChatResponse(ChatResponse other) => CopyFrom(other);

        [Preserve]
        [JsonConstructor]
        public ChatResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created")] int createdAt,
            [JsonProperty("model")] string model,
            [JsonProperty("system_fingerprint")] string systemFingerprint,
            [JsonProperty("usage")] Usage usage,
            [JsonProperty("choices")] IReadOnlyList<Choice> choices)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAt;
            Model = model;
            SystemFingerprint = systemFingerprint;
            Usage = usage;
            this.choices = choices.ToList();
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Obsolete("Use CreatedAtUnixTimeSeconds")]
        public int Created => CreatedAtUnixTimeSeconds;

        /// <summary>
        /// The Unix timestamp (in seconds) of when the chat completion was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; private set; }

        /// <summary>
        /// This fingerprint represents the backend configuration that the model runs with.
        /// Can be used in conjunction with the seed request parameter to understand when
        /// backend changes have been made that might impact determinism.
        /// </summary>
        [Preserve]
        [JsonProperty("system_fingerprint")]
        public string SystemFingerprint { get; private set; }

        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; private set; }

        [Preserve]
        [JsonIgnore]
        private List<Choice> choices;

        [Preserve]
        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices
        {
            get => choices;
            private set => choices = value.ToList();
        }

        [Preserve]
        [JsonIgnore]
        public Choice FirstChoice => Choices?.FirstOrDefault(choice => choice.Index == 0);

        [Preserve]
        public override string ToString() => FirstChoice?.ToString() ?? string.Empty;

        [Preserve]
        public static implicit operator string(ChatResponse response) => response.ToString();

        [Preserve]
        internal void CopyFrom(ChatResponse other)
        {
            if (!string.IsNullOrWhiteSpace(other?.Id))
            {
                Id = other.Id;
            }

            if (!string.IsNullOrWhiteSpace(other?.Object))
            {
                Object = other.Object;
            }

            if (!string.IsNullOrWhiteSpace(other?.Model))
            {
                Model = other.Model;
            }

            if (other?.Usage != null)
            {
                if (Usage == null)
                {
                    Usage = other.Usage;
                }
                else
                {
                    Usage.CopyFrom(other.Usage);
                }
            }

            if (other?.Choices is { Count: > 0 })
            {
                choices ??= new List<Choice>();

                foreach (var otherChoice in other.Choices)
                {
                    if (otherChoice.Index + 1 > choices.Count)
                    {
                        choices.Insert(otherChoice.Index, otherChoice);
                    }

                    choices[otherChoice.Index].CopyFrom(otherChoice);
                }
            }
        }

        public string GetUsage(bool log = true)
        {
            var message = $"{Id} | {Model} | {Usage}";

            if (log)
            {
                Debug.Log(message);
            }

            return message;
        }
    }
}
