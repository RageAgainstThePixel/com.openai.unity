// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.FineTuning
{
    public sealed class FineTuneJobResponse : BaseResponse
    {
        public static implicit operator FineTuneJob(FineTuneJobResponse jobResponse)
            => new FineTuneJob
            {
                Id = jobResponse.Id,
                Object = jobResponse.Object,
                Model = jobResponse.Model,
                CreatedAtUnixTime = jobResponse.CreatedUnixTime,
                Events = jobResponse.Events.ToList(),
                FineTunedModel = jobResponse.FineTunedModel,
                HyperParams = jobResponse.HyperParams,
                OrganizationId = jobResponse.OrganizationId,
                ResultFiles = jobResponse.ResultFiles.ToList(),
                Status = jobResponse.Status,
                ValidationFiles = jobResponse.ValidationFiles.ToList(),
                TrainingFiles = jobResponse.TrainingFiles.ToList(),
                UpdatedAtUnixTime = jobResponse.UpdatedAtUnixTime
            };

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("created_at")]
        public int CreatedUnixTime { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

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
