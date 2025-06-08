// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    [Preserve]
    internal sealed class ImagesResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal ImagesResponse(
            [JsonProperty("created")] long createdAtUnixSeconds,
            [JsonProperty("data")] IReadOnlyList<ImageResult> results,
            [JsonProperty("background")] string background,
            [JsonProperty("output_format")] string outputFormat,
            [JsonProperty("quality")] string quality,
            [JsonProperty("size")] string size,
            [JsonProperty("usage")] TokenUsage usage)
        {
            CreatedAtUnixSeconds = createdAtUnixSeconds;
            Results = results;
            Background = background;
            OutputFormat = outputFormat;
            Quality = quality;
            Size = size;
            Usage = usage;
        }

        [Preserve]
        [JsonProperty("created")]
        public long CreatedAtUnixSeconds { get; }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<ImageResult> Results { get; }

        [Preserve]
        [JsonProperty("background")]
        public string Background { get; }

        [Preserve]
        [JsonProperty("output_format")]
        public string OutputFormat { get; }

        [Preserve]
        [JsonProperty("quality")]
        public string Quality { get; }

        [Preserve]
        [JsonProperty("size")]
        public string Size { get; }

        [Preserve]
        [JsonProperty("usage")]
        public TokenUsage Usage { get; }
    }
}
