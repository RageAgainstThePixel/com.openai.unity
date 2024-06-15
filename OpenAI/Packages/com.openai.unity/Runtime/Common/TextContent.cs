// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class TextContent : IAppendable<TextContent>
    {
        [Preserve]
        public TextContent(string value) => Value = value;

        [Preserve]
        public TextContent(string value, IEnumerable<Annotation> annotations = null)
        {
            Value = value;
            this.annotations = annotations?.ToList();
        }

        [Preserve]
        [JsonConstructor]
        internal TextContent(
            [JsonProperty("index")] int? index,
            [JsonProperty("value")] string value,
            [JsonProperty("annotations")] IEnumerable<Annotation> annotations)
        {
            Index = index;
            Value = value;
            this.annotations = annotations?.ToList();
        }

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; private set; }

        /// <summary>
        /// The data that makes up the text.
        /// </summary>
        [Preserve]
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Include)]
        public string Value { get; private set; }

        private List<Annotation> annotations;

        /// <summary>
        /// Annotations
        /// </summary>
        [Preserve]
        [JsonProperty("annotations")]
        public IReadOnlyList<Annotation> Annotations => annotations;

        [Preserve]
        public static implicit operator TextContent(string value) => new(value);

        [Preserve]
        public static implicit operator string(TextContent content) => content.Value;

        [Preserve]
        public override string ToString() => Value;

        [Preserve]
        public void AppendFrom(TextContent other)
        {
            if (other == null) { return; }

            if (other.Index.HasValue)
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other.Value))
            {
                Value += other.Value;
            }

            if (other is { Annotations: not null })
            {
                annotations ??= new List<Annotation>();
                annotations.AppendFrom(other.Annotations);
            }
        }
    }
}
