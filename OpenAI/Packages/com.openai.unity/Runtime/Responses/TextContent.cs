// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class TextContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal TextContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("text")] string text,
            [JsonProperty("annotations")] IReadOnlyList<Annotation> annotations)
        {
            Type = type;
            Text = text;
            Annotations = annotations ?? new List<Annotation>();
        }

        [Preserve]
        public TextContent(string text)
        {
            Type = ResponseContentType.InputText;
            Text = text;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; }

        [Preserve]
        [JsonProperty("annotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<Annotation> Annotations { get; }

        [Preserve]
        public static implicit operator TextContent(string input) => new(input);
    }
}
