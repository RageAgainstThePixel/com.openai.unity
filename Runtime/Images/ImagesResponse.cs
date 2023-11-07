// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    [Preserve]
    internal class ImagesResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        public ImagesResponse(
            [JsonProperty("created")] int created,
            [JsonProperty("data")] IReadOnlyList<ImageResult> results)
        {
            Created = created;
            Results = results;
        }

        [Preserve]
        [JsonProperty("created")]
        public int Created { get; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<ImageResult> Results { get; }
    }
}
