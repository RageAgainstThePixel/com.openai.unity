// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.VectorStores
{
    public sealed class ChunkingStrategy
    {
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

        [JsonProperty("type")]
        public ChunkingStrategyType Type { get; }

        [JsonProperty("static")]
        public ChunkingStrategyStatic Static { get; }
    }
}
