// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Extensions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ToolCall : IAppendable<ToolCall>, IToolCall
    {
        [Preserve]
        public ToolCall() { }

        [Preserve]
        public ToolCall(string toolCallId, string functionName, JToken functionArguments = null)
        {
            Id = toolCallId;
            Function = new Function(functionName, arguments: functionArguments);
            Type = "function";
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
        [JsonIgnore]
        public bool IsFunction => Type == "function";

        [Preserve]
        [JsonIgnore]
        public string CallId => Id;

        [Preserve]
        [JsonIgnore]
        public string Name => Function.Name;

        [Preserve]
        [JsonIgnore]
        public JToken Arguments => Function.Arguments;

        [Preserve]
        public void AppendFrom(ToolCall other)
        {
            if (other == null) { return; }

            if (!string.IsNullOrWhiteSpace(other.Id))
            {
                Id = other.Id;
            }

            if (other.Index.HasValue)
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other.Type))
            {
                Type = other.Type;
            }

            if (other.Function != null)
            {
                if (Function == null)
                {
                    Function = new Function(other.Function);
                }
                else
                {
                    Function.AppendFrom(other.Function);
                }
            }
        }

        [Preserve]
        public string InvokeFunction()
            => ToolExtensions.InvokeFunction(this);

        [Preserve]
        public T InvokeFunction<T>()
            => ToolExtensions.InvokeFunction<T>(this);

        [Preserve]
        public Task<string> InvokeFunctionAsync(CancellationToken cancellationToken = default)
            => ToolExtensions.InvokeFunctionAsync(this, cancellationToken);

        [Preserve]
        public Task<T> InvokeFunctionAsync<T>(CancellationToken cancellationToken = default)
            => ToolExtensions.InvokeFunctionAsync<T>(this, cancellationToken);
    }
}
