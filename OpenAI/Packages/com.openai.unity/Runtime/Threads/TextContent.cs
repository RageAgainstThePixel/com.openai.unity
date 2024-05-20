// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class TextContent
    {
        [Preserve]
        public TextContent(string value) => Value = value;

        [Preserve]
        [JsonConstructor]
        public TextContent(
            [JsonProperty("value")] string value,
            [JsonProperty("annotations")] IReadOnlyList<Annotation> annotations)
        {
            Value = value;
            Annotations = annotations;
        }

        /// <summary>
        /// The data that makes up the text.
        /// </summary>
        [Preserve]
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include)]
        public string Value { get; }

        /// <summary>
        /// Annotations
        /// </summary>
        [Preserve]
        [JsonProperty("annotations")]
        public IReadOnlyList<Annotation> Annotations { get; }

        [Preserve]
        public static implicit operator TextContent(string value) => new(value);

        [Preserve]
        public static implicit operator string(TextContent text) => text?.ToString();

        public override string ToString() => Value;
    }
}
