// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System;
using System.Collections.Generic;

namespace OpenAI.FineTuning
{
    public sealed class FineTuneJob
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAtUnixTime { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [JsonProperty("events")]
        public IReadOnlyList<Event> Events { get; set; }

        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; set; }

        [JsonProperty("hyperparams")]
        public HyperParams HyperParams { get; set; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }

        [JsonProperty("result_files")]
        public IReadOnlyList<FileData> ResultFiles { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("validation_files")]
        public IReadOnlyList<FileData> ValidationFiles { get; set; }

        [JsonProperty("training_files")]
        public IReadOnlyList<FileData> TrainingFiles { get; set; }

        [JsonProperty("updated_at")]
        public int UpdatedAtUnixTime { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt => DateTimeOffset.FromUnixTimeSeconds(UpdatedAtUnixTime).DateTime;
    }
}
