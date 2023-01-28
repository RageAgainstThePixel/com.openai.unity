// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.FineTuning
{
    public sealed class HyperParams
    {
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        [JsonProperty("learning_rate_multiplier")]
        public double? LearningRateMultiplier { get; set; }

        [JsonProperty("n_epochs")]
        public int Epochs { get; set; }

        [JsonProperty("prompt_loss_weight")]
        public double PromptLossWeight { get; set; }
    }
}
