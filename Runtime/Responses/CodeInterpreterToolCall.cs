// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Threads;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool call to run code.
    /// </summary>
    [Preserve]
    public sealed class CodeInterpreterToolCall : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal CodeInterpreterToolCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("code")] string code,
            [JsonProperty("results")] List<CodeInterpreterOutputs> results,
            [JsonProperty("container_id")] string containerId)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Code = code;
            Results = results;
            ContainerId = containerId;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        /// <summary>
        /// The code to run.
        /// </summary>
        [Preserve]
        [JsonProperty("code")]
        public string Code { get; internal set; }

        private string delta;

        [Preserve]
        [JsonIgnore]
        public string Delta
        {
            get => delta;
            internal set
            {
                if (value == null)
                {
                    delta = null;
                }
                else
                {
                    delta += value;
                }
            }
        }

        /// <summary>
        /// The results of the code interpreter tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("results")]
        public IReadOnlyList<CodeInterpreterOutputs> Results { get; }

        /// <summary>
        /// The ID of the container used to run the code.
        /// </summary>
        [Preserve]
        [JsonProperty("container_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ContainerId { get; }

        [Preserve]
        public override string ToString()
            => Delta ?? Code ?? string.Empty;
    }
}
