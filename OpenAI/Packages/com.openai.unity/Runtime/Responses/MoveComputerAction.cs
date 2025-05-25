// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A mouse move action.
    /// </summary>
    [Preserve]
    public sealed class MoveComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal MoveComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("x")] int x,
            [JsonProperty("y")] int y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        [Preserve]
        public MoveComputerAction(Coordinate position)
        {
            Type = ComputerActionType.Move;
            X = position.X;
            Y = position.Y;
        }

        /// <summary>
        /// Specifies the event type. For a move action, this property is always set to `move`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// The x-coordinate to move to.
        /// </summary>
        [Preserve]
        [JsonProperty("x")]
        public int X { get; }

        /// <summary>
        /// The y-coordinate to move to.
        /// </summary>
        [Preserve]
        [JsonProperty("y")]
        public int Y { get; }
    }
}
