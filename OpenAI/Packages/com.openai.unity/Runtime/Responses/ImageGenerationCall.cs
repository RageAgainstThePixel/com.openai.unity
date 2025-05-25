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
    public sealed class ImageGenerationCall : IResponseItem
    {
        [Preserve]
        [JsonConstructor]
        internal ImageGenerationCall(
            [JsonProperty("id")] string id,
            [JsonProperty("type")] ResponseItemType type,
            [JsonProperty("object")] string @object,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("result")] string result)
        {
            Id = id;
            Type = type;
            Object = @object;
            Status = status;
            Result = result;
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
        [JsonIgnore]
        public Texture2D Image { get; internal set; }
    }
}
