// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class FineTuneJob
    {
        [Preserve]
        [JsonConstructor]
        public FineTuneJob(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTime,
            [JsonProperty("finished_at")] int finishedAtUnitTime,
            [JsonProperty("model")] string model,
            [JsonProperty("fine_tuned_model")] string fineTunedModel,
            [JsonProperty("organization_id")] string organizationId,
            [JsonProperty("status")] JobStatus status,
            [JsonProperty("hyperparameters")] HyperParams hyperParameters,
            [JsonProperty("training_file")] FileData trainingFile,
            [JsonProperty("validation_file")] FileData validationFile,
            [JsonProperty("result_files")] IReadOnlyList<FileData> resultFiles,
            [JsonProperty("trained_tokens")] int trainedTokens)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTime = createdAtUnixTime;
            FinishedAtUnixTime = finishedAtUnitTime;
            Model = model;
            FineTunedModel = fineTunedModel;
            OrganizationId = organizationId;
            Status = status;
            HyperParameters = hyperParameters;
            TrainingFile = trainingFile;
            ValidationFile = validationFile;
            ResultFiles = resultFiles;
            TrainedTokens = trainedTokens;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTime).DateTime;

        [Preserve]
        [JsonProperty("finished_at")]
        public int FinishedAtUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime FinishedAt => DateTimeOffset.FromUnixTimeSeconds(FinishedAtUnixTime).DateTime;

        [JsonIgnore]
        [Obsolete("use FinishedAtUnixTime instead")]
        public int UpdatedAtUnixTime { get; }

        [JsonIgnore]
        [Obsolete("Use FinishedAt instead")]
        public DateTime UpdatedAt => DateTimeOffset.FromUnixTimeSeconds(UpdatedAtUnixTime).DateTime;

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        [Preserve]
        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; }

        [Preserve]
        [JsonProperty("organization_id")]
        public string OrganizationId { get; }

        [Preserve]
        [JsonProperty("status")]
        public JobStatus Status { get; set; }

        [Preserve]
        [JsonProperty("hyperparameters")]
        public HyperParams HyperParameters { get; }

        [JsonIgnore]
        [Obsolete("Use HyperParameters instead")]
        public HyperParams HyperParams => HyperParameters;

        [Preserve]
        [JsonProperty("training_file")]
        public FileData TrainingFile { get; }

        [JsonIgnore]
        [Obsolete("use TrainingFile instead")]
        public IReadOnlyList<FileData> TrainingFiles { get; }

        [Preserve]
        [JsonProperty("validation_file")]
        public FileData ValidationFile { get; }

        [JsonIgnore]
        [Obsolete("Use ValidationFile instead")]
        public IReadOnlyList<FileData> ValidationFiles { get; }

        [Preserve]
        [JsonProperty("result_files")]
        public IReadOnlyList<FileData> ResultFiles { get; }

        [JsonIgnore]
        [Obsolete("Removed. Get events using endpoint call.")]
        public IReadOnlyList<Event> Events { get; }

        [JsonProperty("trained_tokens")]
        public int TrainedTokens { get; }

        public static implicit operator string(FineTuneJob job) => job.Id;
    }
}
