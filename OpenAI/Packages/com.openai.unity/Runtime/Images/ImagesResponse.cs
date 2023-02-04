// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Images
{
    internal class ImagesResponse : BaseResponse
    {
        [JsonConstructor]
        public ImagesResponse(int created, IReadOnlyList<ImageResult> data)
        {
            Created = created;
            Data = data;
        }

        [JsonProperty("created")]
        public int Created { get; }

        [JsonProperty("data")]
        public IReadOnlyList<ImageResult> Data { get; }
    }
}
