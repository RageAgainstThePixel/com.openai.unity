// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    public sealed class TokenDetails
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
        public int Text { get; }

        /// <summary>
        /// The number of audio tokens used in the Response.
        /// </summary>
        [Preserve]
        [JsonProperty("audio_tokens")]
        public int Audio { get; }
    }
}
