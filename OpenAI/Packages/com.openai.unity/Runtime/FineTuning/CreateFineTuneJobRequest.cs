// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.FineTuning
{
    public sealed class CreateFineTuneJobRequest
    {
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

        [JsonProperty("training_file")]
        public string TrainingFileId { get; set; }

        [JsonProperty("validation_file")]
        public string ValidationFileId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("n_epochs")]
        public int Epochs { get; set; }

        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; }

        [JsonProperty("prompt_loss_weight")]
        public double PromptLossWeight { get; set; }

        [JsonProperty("compute_classification_metrics")]
        public bool ComputeClassificationMetrics { get; set; }

        [JsonProperty("classification_n_classes")]
        public int? ClassificationNClasses { get; set; }

        [JsonProperty("classification_positive_class")]
        public string ClassificationPositiveClasses { get; set; }

        [JsonProperty("classification_betas")]
        public IReadOnlyList<double> ClassificationBetas { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }
}
