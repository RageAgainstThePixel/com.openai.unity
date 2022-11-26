// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.Images
{
    internal class ImageGenerationResponse : BaseResponse
    {
        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("data")]
        public List<ImageResult> Data { get; set; }
    }
}
