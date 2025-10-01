// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// An image generation request made by the model.
    /// </summary>
    [Preserve]
    public sealed class ImageGenerationCall : BaseResponse, IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ImageGenerationCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("result")] string result,
            [JsonProperty("partial_image_b64")] string partialImageResult = null,
            [JsonProperty("output_format")] string outputFormat = null,
            [JsonProperty("revised_prompt")] string revisedPrompt = null,
            [JsonProperty("background")] string background = null,
            [JsonProperty("size")] string size = null,
            [JsonProperty("quality")] string quality = null)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Result = result;
            PartialImageResult = partialImageResult;
            OutputFormat = outputFormat;
            RevisedPrompt = revisedPrompt;
            Background = background;
            Size = size;
            Quality = quality;
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
        /// The generated image encoded in base64.
        /// </summary>
        [Preserve]
        [JsonProperty("result")]
        public string Result { get; }

        [Preserve]
        [JsonProperty("partial_image_b64")]
        public string PartialImageResult { get; }

        [Preserve]
        [JsonProperty("output_format")]
        public string OutputFormat { get; }

        [Preserve]
        [JsonProperty("revised_prompt")]
        public string RevisedPrompt { get; }

        [Preserve]
        [JsonProperty("background")]
        public string Background { get; }

        [Preserve]
        [JsonProperty("size")]
        public string Size { get; }

        [Preserve]
        [JsonProperty("quality")]
        public string Quality { get; }

        [Preserve]
        [JsonIgnore]
        public Texture2D Image { get; internal set; }
    }
}
