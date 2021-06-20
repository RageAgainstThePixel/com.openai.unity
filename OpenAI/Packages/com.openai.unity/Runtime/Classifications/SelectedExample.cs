// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI
{
    public sealed class SelectedExample
    {
        [JsonProperty("document")]
        public int Document { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
