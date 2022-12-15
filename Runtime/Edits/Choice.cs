// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Edits
{
    public sealed class Choice
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets the main text of this completion
        /// </summary>
        public override string ToString() => Text;

        public static implicit operator string(Choice choice) => choice.Text;
    }
}
