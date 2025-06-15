// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ReasoningSummary : IServerSentEvent
    {
        [Preserve]
        [JsonConstructor]
        internal ReasoningSummary(
            [JsonProperty("type")] string type,
            [JsonProperty("text")] string text)
        {
            Type = type;
            Text = text;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// A short summary of the reasoning used by the model when generating the response.
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
            => Text;
    }
}
