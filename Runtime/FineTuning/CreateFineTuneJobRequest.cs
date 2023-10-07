// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class CreateFineTuneJobRequest
    {
        [Preserve]
        public CreateFineTuneJobRequest(
            string trainingFileId,
            string validationFileId = null,
            string model = null,
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
            Model = model ?? "curie";
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
        [JsonProperty("training_file")]
        public string TrainingFileId { get; set; }

        [Preserve]
        [JsonProperty("validation_file")]
        public string ValidationFileId { get; set; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; set; }

        [Preserve]
        [JsonProperty("n_epochs")]
        public int Epochs { get; set; }

        [Preserve]
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        [Preserve]
        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; }

        [Preserve]
        [JsonProperty("prompt_loss_weight")]
        public double PromptLossWeight { get; set; }

        [Preserve]
        [JsonProperty("compute_classification_metrics")]
        public bool ComputeClassificationMetrics { get; set; }

        [Preserve]
        [JsonProperty("classification_n_classes")]
        public int? ClassificationNClasses { get; set; }

        [Preserve]
        [JsonProperty("classification_positive_class")]
        public string ClassificationPositiveClasses { get; set; }

        [Preserve]
        [JsonProperty("classification_betas")]
        public IReadOnlyList<double> ClassificationBetas { get; set; }

        [Preserve]
        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }
}
