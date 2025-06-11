// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class RefusalContent : BaseResponse, IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal RefusalContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("refusal")] string refusal)
        {
            Type = type;
            Refusal = refusal;
        }

        /// <summary>
        /// The type of the refusal. Always `refusal`.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public ResponseContentType Type { get; }

        /// <summary>
        /// The refusal explanation from the model.
        /// </summary>
        [Preserve]
        [JsonProperty("refusal")]
        public string Refusal { get; internal set; }

        [JsonIgnore]
        public string Delta { get; internal set; }

        [JsonIgnore]
        public string Object => Type.ToString();
    }
}
