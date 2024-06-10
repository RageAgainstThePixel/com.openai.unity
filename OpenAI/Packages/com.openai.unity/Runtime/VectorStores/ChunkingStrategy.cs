// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    [Preserve]
    public sealed class ChunkingStrategy
    {
        [Preserve]
        [JsonConstructor]
        internal ChunkingStrategy(
            [JsonProperty("type")] ChunkingStrategyType type,
            [JsonProperty("static")] ChunkingStrategyStatic @static = null)
        {
            Type = type;
            Static = @static;
        }

        [Preserve]
        public ChunkingStrategy(ChunkingStrategyType type)
        {
            Type = type;

            switch (Type)
            {
                case ChunkingStrategyType.Static:
                    Static = new ChunkingStrategyStatic();
                    break;
            }
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ChunkingStrategyType Type { get; }

        [Preserve]
        [JsonProperty("static", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChunkingStrategyStatic Static { get; }
    }
}
