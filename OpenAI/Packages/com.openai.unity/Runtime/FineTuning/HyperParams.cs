// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class HyperParams
    {
        [Preserve]
        [JsonConstructor]
        public HyperParams(
            [JsonProperty("batch_size")] int? batchSize,
            [JsonProperty("learning_rate_multiplier")] double? learningRateMultiplier,
            [JsonProperty("n_epochs")] int epochs,
            [JsonProperty("prompt_loss_weight")] double promptLossWeight)
        {
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
            Epochs = epochs;
            PromptLossWeight = promptLossWeight;
        }

        [Preserve]
        [JsonProperty("batch_size")]
        public int? BatchSize { get; }

        [Preserve]
        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; }

        [Preserve]
        [JsonProperty("n_epochs")]
        public int Epochs { get; }

        [Preserve]
        [JsonProperty("prompt_loss_weight")]
        public double PromptLossWeight { get; }
    }
}
