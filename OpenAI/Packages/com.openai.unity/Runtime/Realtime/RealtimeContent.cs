// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeContent
    {
        /// <summary>
        /// The content type ("text", "audio", "input_text", "input_audio").
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public RealtimeContentType Type { get; private set; }

        /// <summary>
        /// The text content.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; private set; }

        /// <summary>
        /// Base64-encoded audio data.
        /// </summary>
        [Preserve]
        [JsonProperty("audio")]
        public string Audio { get; private set; }

        /// <summary>
        /// The transcript of the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("transcript")]
        public string Transcript { get; private set; }
    }
}
