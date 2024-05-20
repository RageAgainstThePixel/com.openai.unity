// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    public sealed class FineTuneJobResponse : BaseResponse
    {
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; private set; }

        [Preserve]
        [JsonProperty("created_at")]
        public int? CreateAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? CreatedAt
            => CreateAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CreateAtUnixTimeSeconds.Value).DateTime
                : null;

        [Preserve]
        [JsonProperty("finished_at")]
        public int? FinishedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? FinishedAt
            => FinishedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(FinishedAtUnixTimeSeconds.Value).DateTime
                : null;

        [Preserve]
        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; private set; }

        [Preserve]
        [JsonProperty("organization_id")]
        public string OrganizationId { get; private set; }

        [Preserve]
        [JsonProperty("result_files")]
        public IReadOnlyList<string> ResultFiles { get; private set; }

        [Preserve]
        [JsonProperty("status")]
        public JobStatus Status { get; private set; }

        [Preserve]
        [JsonProperty("validation_file")]
        public string ValidationFile { get; private set; }

        [Preserve]
        [JsonProperty("training_file")]
        public string TrainingFile { get; private set; }

        [Preserve]
        [JsonProperty("hyperparameters")]
        public HyperParams HyperParameters { get; private set; }

        [Preserve]
        [JsonProperty("trained_tokens")]
        public int? TrainedTokens { get; private set; }

        private List<EventResponse> events = new();

        [JsonIgnore]
        public IReadOnlyList<EventResponse> Events
        {
            get => events;
            internal set
            {
                events = value?.ToList() ?? new List<EventResponse>();

                foreach (var @event in events)
                {
                    @event.Client = Client;
                }
            }
        }

        public static implicit operator string(FineTuneJobResponse job) => job?.ToString();

        public override string ToString() => Id;
    }
}
