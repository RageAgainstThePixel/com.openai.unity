// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    internal sealed class FineTuneJobResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public FineTuneJobResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("model")] string model,
            [JsonProperty("created_at")] int createdUnixTime,
            [JsonProperty("events")] IReadOnlyList<Event> events,
            [JsonProperty("fine_tuned_model")] string fineTunedModel,
            [JsonProperty("hyperparams")] HyperParams hyperParams,
            [JsonProperty("organization_id")] string organizationId,
            [JsonProperty("result_files")] IReadOnlyList<FileData> resultFiles,
            [JsonProperty("status")] string status,
            [JsonProperty("validation_files")] IReadOnlyList<FileData> validationFiles,
            [JsonProperty("training_files")] IReadOnlyList<FileData> trainingFiles,
            [JsonProperty("updated_at")] int updatedAtUnixTime)
        {
            Id = id;
            Object = @object;
            Model = model;
            CreatedUnixTime = createdUnixTime;
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

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedUnixTime).DateTime;

        [Preserve]
        [JsonProperty("events")]
        public IReadOnlyList<Event> Events { get; }

        [Preserve]
        [JsonProperty("fine_tuned_model")]
        public string FineTunedModel { get; }

        [Preserve]
        [JsonProperty("hyperparams")]
        public HyperParams HyperParams { get; }

        [Preserve]
        [JsonProperty("organization_id")]
        public string OrganizationId { get; }

        [Preserve]
        [JsonProperty("result_files")]
        public IReadOnlyList<FileData> ResultFiles { get; }

        [Preserve]
        [JsonProperty("status")]
        public string Status { get; }

        [Preserve]
        [JsonProperty("validation_files")]
        public IReadOnlyList<FileData> ValidationFiles { get; }

        [Preserve]
        [JsonProperty("training_files")]
        public IReadOnlyList<FileData> TrainingFiles { get; }

        [Preserve]
        [JsonProperty("updated_at")]
        public int UpdatedAtUnixTime { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime UpdatedAt => DateTimeOffset.FromUnixTimeSeconds(UpdatedAtUnixTime).DateTime;

        public static implicit operator FineTuneJob(FineTuneJobResponse jobResponse)
            => new FineTuneJob(
                jobResponse.Id,
                jobResponse.Object,
                jobResponse.Model,
                jobResponse.CreatedUnixTime,
                jobResponse.Events.ToList(),
                jobResponse.FineTunedModel,
                jobResponse.HyperParams,
                jobResponse.OrganizationId,
                jobResponse.ResultFiles.ToList(),
                jobResponse.Status,
                jobResponse.ValidationFiles.ToList(),
                jobResponse.TrainingFiles.ToList(),
                jobResponse.UpdatedAtUnixTime);
    }
}
