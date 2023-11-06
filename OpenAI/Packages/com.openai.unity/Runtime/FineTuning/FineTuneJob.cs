// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class FineTuneJob
    {
        public static implicit operator string(FineTuneJob job) => job.Id;

        [Preserve]
        [JsonConstructor]
        public FineTuneJob(
            [JsonProperty("object")] string @object,
            [JsonProperty("id")] string id,
            [JsonProperty("model")] string model,
            [JsonProperty("created_at")] int? createdAtUnixTime,
            [JsonProperty("finished_at")] int? finishedAtUnixTime,
            [JsonProperty("fine_tuned_model")] string fineTunedModel,
            [JsonProperty("organization_id")] string organizationId,
            [JsonProperty("result_files")] IReadOnlyList<string> resultFiles,
            [JsonProperty("status")] JobStatus status,
            [JsonProperty("validation_file")] string validationFile,
            [JsonProperty("training_file")] string trainingFile,
            [JsonProperty("hyperparameters")] HyperParams hyperParameters,
            [JsonProperty("trained_tokens")] int? trainedTokens)
        {
            Object = @object;
            Id = id;
            Model = model;
            CreatedAtUnixTime = createdAtUnixTime ?? 0;
            FinishedAtUnixTime = finishedAtUnixTime ?? 0;
            FineTunedModel = fineTunedModel;
            OrganizationId = organizationId;
            ResultFiles = resultFiles;
            Status = status;
            ValidationFile = validationFile;
            TrainingFile = trainingFile;
            HyperParameters = hyperParameters;
            TrainedTokens = trainedTokens ?? 0;
        }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

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

        [Preserve]
        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; }

        [Preserve]
        [JsonProperty("organization_id")]
        public string OrganizationId { get; }

        [Preserve]
        [JsonProperty("result_files")]
        public IReadOnlyList<string> ResultFiles { get; }

        [Preserve]
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public JobStatus Status { get; set; }

        [Preserve]
        [JsonProperty("validation_file")]
        public string ValidationFile { get; }

        [Preserve]
        [JsonProperty("training_file")]
        public string TrainingFile { get; }

        [Preserve]
        [JsonProperty("hyperparameters")]
        public HyperParams HyperParameters { get; }

        [Preserve]
        [JsonProperty("trained_tokens")]
        public int TrainedTokens { get; }

        [JsonIgnore]
        public IReadOnlyList<Event> Events { get; internal set; } = new List<Event>();
    }
}
