// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class Tool
    {
        [Preserve]
        public Tool() { }

        [Preserve]
        public Tool(Tool other) => CopyFrom(other);

        [Preserve]
        public Tool(Function function)
        {
            Function = function;
            Type = nameof(function);
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; private set; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        [Preserve]
        [JsonProperty("function")]
        public Function Function { get; private set; }

        [Preserve]
        public static implicit operator Tool(Function function) => new Tool(function);

        [Preserve]
        internal void CopyFrom(Tool other)
        {
            if (!string.IsNullOrWhiteSpace(other?.Id))
            {
                Id = other.Id;
            }

            if (other is { Index: not null })
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other?.Type))
            {
                Type = other.Type;
            }

            if (other?.Function != null)
            {
                if (Function == null)
                {
                    Function = new Function(other.Function);
                }
                else
                {
                    Function.CopyFrom(other.Function);
                }
            }
        }
    }
}
