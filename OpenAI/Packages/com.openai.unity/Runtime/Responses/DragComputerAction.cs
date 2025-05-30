// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A drag action.
    /// </summary>
    [Preserve]
    public sealed class DragComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal DragComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("path")] IReadOnlyList<Coordinate> path)
        {
            Type = type;
            Path = path;
        }

        [Preserve]
        public DragComputerAction(IReadOnlyList<Coordinate> path)
        {
            Type = ComputerActionType.Drag;
            Path = path ?? throw new ArgumentNullException(nameof(path), "Path cannot be null.");
        }

        /// <summary>
        /// Specifies the event type. For a drag action, this property is always set to `drag`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// An array of coordinates representing the path of the drag action.
        /// A series of x/y coordinate pairs in the drag path.
        /// </summary>
        [Preserve]
        [JsonProperty("path")]
        public IReadOnlyList<Coordinate> Path { get; }
    }
}
