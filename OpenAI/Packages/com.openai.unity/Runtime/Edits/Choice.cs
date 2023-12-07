// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Edits
{
    [Preserve]
    [Obsolete("Deprecated")]
    public sealed class Choice
    {
        [Preserve]
        [JsonConstructor]
        public Choice(
            [JsonProperty("text")] string text,
            [JsonProperty("index")] int index)
        {
            Text = text;
            Index = index;
        }

        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }

        [Preserve]
        [JsonProperty("index")]
        public int Index { get; }

        /// <summary>
        /// Gets the main text of this completion
        /// </summary>
        public override string ToString() => Text;

        public static implicit operator string(Choice choice) => choice.Text;
    }
}
