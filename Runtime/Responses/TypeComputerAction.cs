// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// An action to type in text.
    /// </summary>
    [Preserve]
    public sealed class TypeComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal TypeComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("text")] string text)
        {
            Type = type;
            Text = text;
        }

        [Preserve]
        public TypeComputerAction(string text)
        {
            Type = ComputerActionType.Type;
            Text = text;
        }

        /// <summary>
        /// Specifies the event type.For a type action, this property is always set to `type`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// The text to type.
        /// </summary>
        [Preserve]
        [JsonProperty("text")]
        public string Text { get; }
    }
}
