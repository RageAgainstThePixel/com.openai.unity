// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class DeletedResponse
    {
        [Preserve]
        [JsonConstructor]
        public DeletedResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("deleted")] bool deleted)
        {
            Id = id;
            Object = @object;
            Deleted = deleted;
        }

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        [Preserve]
        [JsonProperty("deleted")]
        public bool Deleted { get; }
    }
}
