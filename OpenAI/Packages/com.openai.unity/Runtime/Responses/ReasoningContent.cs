// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ReasoningContent : BaseResponse, IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("text")] string text)
        {
            Type = type;
            Text = text;
        }

        /// <summary>
        /// The type of the reasoning text. Always reasoning_text.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public ResponseContentType Type { get; }

        /// <summary>
        /// The reasoning text from the model.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; internal set; }

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
            => Delta ?? Text ?? string.Empty;
    }
}
