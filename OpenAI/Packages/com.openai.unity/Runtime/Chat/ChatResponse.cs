// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
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
            [JsonProperty("created")] int created,
            [JsonProperty("model")] string model,
            [JsonProperty("usage")] Usage usage,
            [JsonProperty("choices")] List<Choice> choices)
        {
            Id = id;
            Object = @object;
            Created = created;
            Model = model;
            Usage = usage;
            this.choices = choices;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; internal set; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; internal set; }

        [Preserve]
        [JsonProperty("created")]
        public int Created { get; internal set; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; internal set; }

        [Preserve]
        [JsonProperty("usage")]
        public Usage Usage { get; internal set; }

        [Preserve]
        [JsonIgnore]
        private List<Choice> choices;

        [Preserve]
        [JsonProperty("choices")]
        public IReadOnlyList<Choice> Choices => choices;

        [Preserve]
        [JsonIgnore]
        public Choice FirstChoice => Choices?.FirstOrDefault(choice => choice.Index == 0);

        public override string ToString() => FirstChoice?.ToString() ?? string.Empty;

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
