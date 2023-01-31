// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System;
using System.Collections.Generic;

namespace OpenAI.FineTuning
{
    public sealed class FineTuneJob
    {
        [JsonConstructor]
        public FineTuneJob(string id, string @object, string model, int createdAtUnixTime, IReadOnlyList<Event> events, string fineTunedModel, HyperParams hyperParams, string organizationId, IReadOnlyList<FileData> resultFiles, string status, IReadOnlyList<FileData> validationFiles, IReadOnlyList<FileData> trainingFiles, int updatedAtUnixTime)
        {
            Id = id;
            Object = @object;
            Model = model;
            CreatedAtUnixTime = createdAtUnixTime;
            Events = events;
            FineTunedModel = fineTunedModel;
            HyperParams = hyperParams;
            OrganizationId = organizationId;
            ResultFiles = resultFiles;
            Status = status;
            ValidationFiles = validationFiles;
            TrainingFiles = trainingFiles;
            UpdatedAtUnixTime = updatedAtUnixTime;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("object")]
        public string Object { get; }

        [JsonProperty("model")]
        public string Model { get; }

        [JsonProperty("created_at")]
        public int CreatedAtUnixTime { get; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [JsonProperty("events")]
        public IReadOnlyList<Event> Events { get; }

        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; }

        [JsonProperty("hyperparams")]
        public HyperParams HyperParams { get; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; }

        [JsonProperty("result_files")]
        public IReadOnlyList<FileData> ResultFiles { get; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("validation_files")]
        public IReadOnlyList<FileData> ValidationFiles { get; }

        [JsonProperty("training_files")]
        public IReadOnlyList<FileData> TrainingFiles { get; }

        [JsonProperty("updated_at")]
        public int UpdatedAtUnixTime { get; }

        [JsonIgnore]
        public DateTime UpdatedAt => DateTimeOffset.FromUnixTimeSeconds(UpdatedAtUnixTime).DateTime;

        public static implicit operator string(FineTuneJob job) => job.Id;
    }
}
