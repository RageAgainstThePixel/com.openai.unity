// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that generates images using a model like gpt-image-1.
    /// </summary>
    public sealed class ImageGenerationTool : ITool
    {
        public static implicit operator Tool(ImageGenerationTool imageGenerationTool) => new(imageGenerationTool as ITool);

        public ImageGenerationTool(
            Model model = null,
            string background = null,
            InputImageMask inputImageMask = null,
            string moderation = null,
            int? outputCompression = null,
            string outputFormat = null,
            int? partialImages = null,
            string quality = null,
            string size = null)
        {
            Model = string.IsNullOrWhiteSpace(model?.Id) ? Models.Model.GPT_Image_1 : model;
            Background = background;
            InputImageMask = inputImageMask;
            Moderation = moderation;
            OutputCompression = outputCompression;
            OutputFormat = outputFormat;
            PartialImages = partialImages;
            Quality = quality;
            Size = size;
        }

        [JsonProperty("type")]
        public string Type => "image_generation";

        /// <summary>
        /// Background type for the generated image. One of transparent, opaque, or auto. Default: auto.
        /// </summary>
        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Background { get; }

        /// <summary>
        /// Optional mask for inpainting. Contains image_url (string, optional) and file_id (string, optional).
        /// </summary>
        [JsonProperty("input_image_mask", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputImageMask InputImageMask { get; }

        /// <summary>
        /// The image generation model to use. Default: gpt-image-1.
        /// </summary>
        [JsonProperty("model", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Model { get; }

        /// <summary>
        /// Moderation level for the generated image. Default: auto.
        /// </summary>
        [JsonProperty("moderation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Moderation { get; }

        /// <summary>
        /// Compression level for the output image. Default: 100.
        /// </summary>
        [JsonProperty("output_compression", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? OutputCompression { get; }

        /// <summary>
        /// The output format of the generated image. One of png, webp, or jpeg. Default: png.
        /// </summary>
        [JsonProperty("output_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OutputFormat { get; }

        /// <summary>
        /// Number of partial images to generate in streaming mode, from 0 (default value) to 3.
        /// </summary>
        [JsonProperty("partial_images", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PartialImages { get; }

        /// <summary>
        /// The quality of the generated image. One of low, medium, high, or auto. Default: auto.
        /// </summary>
        [JsonProperty("quality", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Quality { get; }

        /// <summary>
        /// The size of the generated image. One of 1024x1024, 1024x1536, 1536x1024, or auto. Default: auto.
        /// </summary>
        [JsonProperty("size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Size { get; }
    }
}
