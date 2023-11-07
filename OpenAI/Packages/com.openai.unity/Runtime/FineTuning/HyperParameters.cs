// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class HyperParameters
    {
        [Preserve]
        [JsonConstructor]
        public HyperParameters(
            [JsonProperty("n_epochs")] int? epochs = null,
            [JsonProperty("batch_size")] int? batchSize = null,
            [JsonProperty("learning_rate_multiplier")] int? learningRateMultiplier = null)
        {
            Epochs = epochs;
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
        }

        [Preserve]
        [JsonProperty("n_epochs")]
        public int? Epochs { get; set; }

        [Preserve]
        [JsonProperty("batch_size")]
        public int? BatchSize { get; set; }

        [Preserve]
        [JsonProperty("learning_rate_multiplier")]
        public int? LearningRateMultiplier { get; set; }
    }
}
