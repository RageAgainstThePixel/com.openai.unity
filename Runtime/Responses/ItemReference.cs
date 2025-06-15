// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class ItemReference : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ItemReference(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type)
        {
            Id = id;
            Type = type;
        }

        [Preserve]
        public ItemReference(IResponseItem item)
        {
            Id = item.Id;
            Type = ResponseItemType.ItemReference;
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
    }
}
