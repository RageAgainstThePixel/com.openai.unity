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
            [JsonProperty("n_epochs")] object epochs,
            [JsonProperty("batch_size")] object batchSize,
            [JsonProperty("learning_rate_multiplier")] object learningRateMultiplier)
        {
            Epochs = epochs;
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
        }

        [Preserve]
        [JsonProperty("n_epochs")]
        public object Epochs { get; }

        [Preserve]
        [JsonProperty("batch_size")]
        public object BatchSize { get; }

        [Preserve]
        [JsonProperty("learning_rate_multiplier")]
        public object LearningRateMultiplier { get; }
    }
}
