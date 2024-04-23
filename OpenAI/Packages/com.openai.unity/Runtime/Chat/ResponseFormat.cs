// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class ResponseFormat
    {
        [Preserve]
        public ResponseFormat() => Type = ChatResponseFormat.Text;

        [Preserve]
        public ResponseFormat(ChatResponseFormat format) => Type = format;

        [Preserve]
        [JsonProperty("type")]
        public ChatResponseFormat Type { get; private set; }

        [Preserve]
        public static implicit operator ChatResponseFormat(ResponseFormat format) => format.Type;

        [Preserve]
        public static implicit operator ResponseFormat(ChatResponseFormat format) => new(format);
    }
}
