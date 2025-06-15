// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ListResponse<TObject> : BaseResponse, IListResponse<TObject>
        where TObject : IListItem
    {
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; private set; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<TObject> Items { get; private set; }

        [Preserve]
        [JsonProperty("has_more")]
        public bool HasMore { get; private set; }

        [Preserve]
        [JsonProperty("first_id")]
        public string FirstId { get; private set; }

        [Preserve]
        [JsonProperty("last_id")]
        public string LastId { get; private set; }
    }
}
