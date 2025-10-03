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

        private string delta;

        [Preserve]
        [JsonIgnore]
        public string Delta
        {
            get => delta;
            internal set
            {
                if (value == null)
                {
                    delta = null;
                }
                else
                {
                    delta += value;
                }
            }
        }

        [Preserve]
        [JsonIgnore]
        public string Object => Type.ToString();

        [Preserve]
        public override string ToString()
            => Delta ?? Refusal ?? string.Empty;
    }
}
