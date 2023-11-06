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
        public HyperParams([JsonProperty("n_epochs")] string epochs)
        {
            Epochs = epochs;
        }

        [Preserve]
        [JsonProperty("n_epochs")]
        public string Epochs { get; }
    }
}
