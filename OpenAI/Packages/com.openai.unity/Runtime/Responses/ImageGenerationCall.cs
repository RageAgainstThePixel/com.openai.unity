// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Threading;
using System.Threading.Tasks;
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
            [JsonProperty("partial_image_index")] int? partialImageIndex = null,
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
        [JsonProperty("result", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Result { get; }

        [Preserve]
        [JsonProperty("partial_image_index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PartialImageIndex { get; internal set; }

        [Preserve]
        [JsonProperty("partial_image_b64", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PartialImageResult { get; internal set; }

        [Preserve]
        [JsonProperty("output_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OutputFormat { get; internal set; }

        [Preserve]
        [JsonProperty("revised_prompt", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RevisedPrompt { get; internal set; }

        [Preserve]
        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Background { get; internal set; }

        [Preserve]
        [JsonProperty("size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Size { get; internal set; }

        [Preserve]
        [JsonProperty("quality", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Quality { get; internal set; }

        /// <summary>
        /// Loads the image result as a <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="debug">Optional, enable debug logging.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="Texture2D"/>.</returns>
        [Preserve]
        public async Task<Texture2D> LoadTextureAsync(bool debug = false, CancellationToken cancellationToken = default)
        {
            var image64 = string.IsNullOrWhiteSpace(Result) ? PartialImageResult : Result;
            if (string.IsNullOrWhiteSpace(image64)) { return null; }
            var (texture, _) = await TextureExtensions.ConvertFromBase64Async(image64, debug, cancellationToken);
            return texture;
        }
    }
}
