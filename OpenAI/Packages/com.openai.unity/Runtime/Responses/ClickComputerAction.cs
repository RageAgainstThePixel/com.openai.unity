// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A click action.
    /// </summary>
    public sealed class ClickComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal ClickComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("button")] ComputerClickButtonType button,
            [JsonProperty("x")] int x,
            [JsonProperty("y")] int y)
        {
            Type = type;
            Button = button;
            X = x;
            Y = y;
        }

        [Preserve]
        public ClickComputerAction(ComputerClickButtonType button, Coordinate position)
        {
            Type = ComputerActionType.Click;
            Button = button;
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Specifies the event type. For a click action, this property is always set to `click`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// Indicates which mouse button was pressed during the click.
        /// </summary>
        [Preserve]
        [JsonProperty("button")]
        public ComputerClickButtonType Button { get; }

        /// <summary>
        /// The x-coordinate where the click occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("x")]
        public int X { get; }

        /// <summary>
        /// The y-coordinate where the click occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("y")]
        public int Y { get; }
    }
}
