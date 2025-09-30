// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ReasoningContent : IServerSentEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningContent(
            [JsonProperty("type")] string type,
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
        public string Type { get; }

        /// <summary>
        /// The reasoning text from the model.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; internal set; }

        [Preserve]
        [JsonIgnore]
        public string Delta { get; internal set; }

        [Preserve]
        [JsonIgnore]
        public string Object
            => Type;

        [Preserve]
        public string ToJsonString()
            => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);

        [Preserve]
        public override string ToString()
            => string.IsNullOrWhiteSpace(Text) ? Delta : Text;
    }
}
