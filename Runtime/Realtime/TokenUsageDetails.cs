// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class TokenUsageDetails
    {
        /// <summary>
        /// The number of cached tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("cached_tokens")]
        public int? CachedTokens { get; }

        /// <summary>
        /// The number of text tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("text_tokens")]
        public int? TextTokens { get; }

        /// <summary>
        /// The number of audio tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_tokens")]
        public int? AudioTokens { get; }

        [Preserve]
        [JsonProperty("image_tokens")]
        public int? ImageTokens { get; }
    }
}
