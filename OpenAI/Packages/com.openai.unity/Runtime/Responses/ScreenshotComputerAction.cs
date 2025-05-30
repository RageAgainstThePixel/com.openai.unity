// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A screenshot action.
    /// </summary>
    [Preserve]
    public sealed class ScreenshotComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal ScreenshotComputerAction([JsonProperty("type")] ComputerActionType type)
        {
            Type = type;
        }

        [Preserve]
        public ScreenshotComputerAction()
        {
            Type = ComputerActionType.Screenshot;
        }

        /// <summary>
        /// Specifies the event type.For a screenshot action, this property is always set to `screenshot`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }
    }
}
