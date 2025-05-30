// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A wait action.
    /// </summary>
    [Preserve]
    public sealed class WaitComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal WaitComputerAction([JsonProperty("type")] ComputerActionType type)
        {
            Type = type;
        }

        [Preserve]
        public WaitComputerAction()
        {
            Type = ComputerActionType.Wait;
        }

        /// <summary>
        /// Specifies the event type.For a wait action, this property is always set to `wait`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }
    }
}
