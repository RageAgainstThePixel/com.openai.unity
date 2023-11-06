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
            [JsonProperty("n_epochs")] string epochs,
            [JsonProperty("batch_size")] string batchSize,
            [JsonProperty("learning_rate_multiplier")] string learningRateMultiplier)
        {
            Epochs = epochs;
            BatchSize = batchSize;
            LearningRateMultiplier = learningRateMultiplier;
        }

        [Preserve]
        [JsonProperty("n_epochs")]
        public string Epochs { get; }

        [Preserve]
        [JsonProperty("batch_size")]
        public string BatchSize { get; }

        [Preserve]
        [JsonProperty("learning_rate_multiplier")]
        public string LearningRateMultiplier { get; }
    }
}
