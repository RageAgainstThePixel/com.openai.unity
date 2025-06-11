// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class LocalShellCallOutput : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal LocalShellCallOutput(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("output")] string output)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Output = output;
        }

        [Preserve]
        public LocalShellCallOutput(string output)
        {
            Type = ResponseItemType.LocalShellCallOutput;
            Output = output;
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
        /// A JSON string of the output of the local shell tool call.
        /// </summary>
        [Preserve]
        [JsonProperty("output")]
        public string Output { get; }
    }
}
