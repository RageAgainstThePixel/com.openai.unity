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
        public HyperParameters([JsonProperty("n_epochs")] int? epochs = null)
        {
            Epochs = epochs;
        }

        [Preserve]
        [JsonProperty("n_epochs")]
        public int? Epochs { get; set; }
    }
}
