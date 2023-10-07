// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class CreateFineTuneJobRequest
    {
        [Preserve]
        public CreateFineTuneJobRequest(
            Model model,
            string trainingFileId,
            HyperParameters hyperParameters = null,
            string suffix = null,
            string validationFileId = null)
        {
            Model = model ?? Models.Model.GPT3_5_Turbo;
            TrainingFileId = trainingFileId;
            HyperParameters = hyperParameters;
            Suffix = suffix;
            ValidationFileId = validationFileId;
        }

        [Obsolete("use new constructor")]
        public CreateFineTuneJobRequest(
            string trainingFileId,
            string model = null,
            string validationFileId = null,
            uint epochs = 4,
            int? batchSize = null,
            double? learningRateMultiplier = null,
            double promptLossWeight = 0.01d,
            bool computeClassificationMetrics = false,
            int? classificationNClasses = null,
            string classificationPositiveClasses = null,
            IReadOnlyList<double> classificationBetas = null,
            string suffix = null)
        {
            TrainingFileId = trainingFileId;
            ValidationFileId = validationFileId;
            Model = model ?? "gpt-3.5-turbo";
            Epochs = (int)epochs;
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
            PromptLossWeight = promptLossWeight;
            ComputeClassificationMetrics = computeClassificationMetrics;
            ClassificationNClasses = classificationNClasses;
            ClassificationPositiveClasses = classificationPositiveClasses;
            ClassificationBetas = classificationBetas;
            Suffix = suffix;
        }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; set; }

        [Preserve]
        [JsonProperty("training_file")]
        public string TrainingFileId { get; set; }

        [Preserve]
        [JsonProperty("hyperparameters")]
        public HyperParameters HyperParameters { get; set; }

        [Preserve]
        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [Preserve]
        [JsonProperty("validation_file")]
        public string ValidationFileId { get; set; }

        #region Obsolete

        [JsonIgnore]
        [Obsolete("use HyperParameters")]
        public int Epochs { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public int? BatchSize { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public double? LearningRateMultiplier { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public double PromptLossWeight { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public bool ComputeClassificationMetrics { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public int? ClassificationNClasses { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public string ClassificationPositiveClasses { get; set; }

        [JsonIgnore]
        [Obsolete("removed")]
        public IReadOnlyList<double> ClassificationBetas { get; set; }

        #endregion Obsolete
    }
}
