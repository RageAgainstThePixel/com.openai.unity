// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A collection of keypresses the model would like to perform.
    /// </summary>
    [Preserve]
    public sealed class KeyPressComputerAction : IComputerAction
    {
        [Preserve]
        [JsonConstructor]
        internal KeyPressComputerAction(
            [JsonProperty("type")] ComputerActionType type,
            [JsonProperty("keys")] IReadOnlyList<string> keys)
        {
            Type = type;
            Keys = keys;
        }

        [Preserve]
        public KeyPressComputerAction(IEnumerable<string> keys)
        {
            Type = ComputerActionType.KeyPress;
            Keys = keys?.ToList() ?? throw new ArgumentNullException(nameof(keys), "Keys cannot be null.");
        }

        /// <summary>
        /// Specifies the event type. For a keypress action, this property is always set to `keypress`.
        /// </summary>
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ComputerActionType Type { get; }

        /// <summary>
        /// The combination of keys the model is requesting to be pressed.
        /// This is an array of strings, each representing a key.
        /// </summary>
        [Preserve]
        [JsonProperty("keys")]
        public IReadOnlyList<string> Keys { get; }
    }
}
