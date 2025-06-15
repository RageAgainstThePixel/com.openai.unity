// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public struct Coordinate
    {
        [Preserve]
        [JsonConstructor]
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        [Preserve]
        [JsonProperty("x")]
        public int X { get; }

        [Preserve]
        [JsonProperty("y")]
        public int Y { get; }

        [Preserve]
        public static implicit operator Vector2Int(Coordinate coordinate)
            => new(coordinate.X, coordinate.Y);

        [Preserve]
        public static implicit operator Coordinate(Vector2Int position)
            => new(position.x, position.y);
    }
}
