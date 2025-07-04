// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class TextContent : BaseResponse, IResponseContent
    {
        [Preserve]
        public static implicit operator TextContent(string input) => new(input);

        [Preserve]
        [JsonConstructor]
        internal TextContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("text")] string text,
            [JsonProperty("annotations")] IReadOnlyList<IAnnotation> annotations,
            IReadOnlyList<LogProbInfo> logProbs)
        {
            Type = type;
            Text = text;
            Annotations = annotations;
            LogProbs = logProbs;
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
        public string Text { get; internal set; }

        private List<IAnnotation> annotations;

        [Preserve]
        [JsonProperty("annotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IAnnotation> Annotations
        {
            get => annotations;
            private set => annotations = value?.ToList();
        }

        [Preserve]
        [JsonProperty("logprobs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<LogProbInfo> LogProbs { get; }

        [Preserve]
        [JsonProperty("delta", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Delta { get; internal set; }

        [JsonIgnore]
        public string Object => Type.ToString();

        [Preserve]
        internal void InsertAnnotation(IAnnotation item, int index)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            annotations ??= new();

            if (index > annotations.Count)
            {
                for (var i = annotations.Count; i < index; i++)
                {
                    annotations.Add(null);
                }
            }

            annotations.Insert(index, item);
        }

        [Preserve]
        public override string ToString()
            => Text ?? string.Empty;
    }
}
