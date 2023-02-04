// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.FineTuning
{
    public sealed class HyperParams
    {
        [JsonConstructor]
        public HyperParams(int? batchSize, double? learningRateMultiplier, int epochs, double promptLossWeight)
        {
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
            Epochs = epochs;
            PromptLossWeight = promptLossWeight;
        }

        [JsonProperty("batch_size")]
        public int? BatchSize { get; }

        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; }

        [JsonProperty("n_epochs")]
        public int Epochs { get; }

        [JsonProperty("prompt_loss_weight")]
        public double PromptLossWeight { get; }
    }
}
