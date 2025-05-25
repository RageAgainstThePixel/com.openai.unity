// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A scroll action.
    /// </summary>
    [Preserve]
    public sealed class ScrollComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal ScrollComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("x")] int x,
            [JsonProperty("y")] int y,
            [JsonProperty("scroll_x")] int scrollX,
            [JsonProperty("scroll_y")] int scrollY)
        {
            Type = type;
            X = x;
            Y = y;
            ScrollX = scrollX;
            ScrollY = scrollY;
        }

        [Preserve]
        public ScrollComputerAction(Coordinate position, Coordinate scrollDistance)
        {
            Type = ComputerActionType.Scroll;
            X = position.X;
            Y = position.Y;
            ScrollX = scrollDistance.X;
            ScrollY = scrollDistance.Y;
        }

        /// <summary>
        /// Specifies the event type.For a scroll action, this property is always set to `scroll`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// The x-coordinate where the scroll occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("x")]
        public int X { get; }

        /// <summary>
        /// The y-coordinate where the scroll occurred.
        /// </summary>
        [Preserve]
        [JsonProperty("y")]
        public int Y { get; }

        /// <summary>
        /// The horizontal scroll distance.
        /// </summary>
        [Preserve]
        [JsonProperty("scroll_x")]
        public int ScrollX { get; }

        /// <summary>
        /// The vertical scroll distance.
        /// </summary>
        [Preserve]
        [JsonProperty("scroll_y")]
        public int ScrollY { get; }
    }
}
