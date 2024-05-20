// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ResponseFormatObject
    {
        [Preserve]
        public ResponseFormatObject() => Type = ResponseFormat.Text;

        [Preserve]
        [JsonConstructor]
        public ResponseFormatObject([JsonProperty("type")] ResponseFormat type) => Type = type;

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseFormat Type { get; private set; }

        [Preserve]
        public static implicit operator ResponseFormatObject(ResponseFormat type) => new(type);

        [Preserve]
        public static implicit operator ResponseFormat(ResponseFormatObject format) => format.Type;
    }
}
