// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI
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
            [JsonProperty("annotations")] IEnumerable<Annotation> annotations)
        {
            Value = value;
            Annotations = annotations?.ToList();
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
        public static implicit operator string(TextContent content) => content.Value;

        [Preserve]
        public override string ToString() => Value;
    }
}
