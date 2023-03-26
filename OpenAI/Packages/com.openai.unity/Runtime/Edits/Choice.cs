// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;

namespace OpenAI.Edits
{
    [Obsolete]
    public sealed class Choice
    {
        [JsonConstructor]
        public Choice(string text, int index)
        {
            Text = text;
            Index = index;
        }

        [JsonProperty("text")]
        public string Text { get; }

        [JsonProperty("index")]
        public int Index { get; }

        /// <summary>
        /// Gets the main text of this completion
        /// </summary>
        public override string ToString() => Text;

        public static implicit operator string(Choice choice) => choice.Text;
    }
}
