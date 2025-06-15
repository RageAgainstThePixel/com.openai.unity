// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A double click action.
    /// </summary>
    [Preserve]
    public sealed class DoubleClickComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal DoubleClickComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("x")] int x,
            [JsonProperty("y")] int y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        [Preserve]
        public DoubleClickComputerAction(Coordinate position)
        {
            Type = ComputerActionType.DoubleClick;
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Specifies the event type.For a double click action, this property is always set to `double_click`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// The x-coordinate where the double click occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("x", DefaultValueHandling = DefaultValueHandling.Include)]
        public int X { get; }

        /// <summary>
        /// The y-coordinate where the double click occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("y", DefaultValueHandling = DefaultValueHandling.Include)]
        public int Y { get; }
    }
}
