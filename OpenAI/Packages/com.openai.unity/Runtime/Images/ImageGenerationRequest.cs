// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    [Preserve]
    public sealed class ImageGenerationRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prompt">
        /// A text description of the desired image(s). The maximum length is 32000 characters for `gpt-image-1`, 1000 characters for `dall-e-2` and 4000 characters for `dall-e-3`.
        /// </param>
        /// <param name="model">
        /// The model to use for image generation. One of `dall-e-2`, `dall-e-3`, or `gpt-image-1`. Defaults to `dall-e-2` unless a parameter specific to `gpt-image-1` is used.
        /// </param>
        /// <param name="numberOfResults">
        /// The number of images to generate.
        /// Must be between 1 and 10. For dall-e-3, only n=1 is supported.
        /// </param>
        /// <param name="quality">
        /// The quality of the image that will be generated.<br/>
        /// - `auto` (default value) will automatically select the best quality for the given model.<br/>
        /// - `high`, `medium` and `low` are supported for `gpt-image-1`.<br/>
        /// - `hd` and `standard` are supported for `dall-e-3`.<br/>
        /// - `standard` is the only option for `dall-e-2`.
        /// </param>
        /// <param name="responseFormat">
        /// The format in which generated images with `dall-e-2` and `dall-e-3`
        /// are returned. Must be one of `url` or `b64_json`. URLs are only
        /// valid for 60 minutes after the image has been generated.
        /// `gpt-image-1` does not support urls and only supports base64-encoded images.
        /// </param>
        /// <param name="outputFormat">
        /// The format in which the generated images are returned.
        /// Must be one of `png`, `jpeg`, or `webp`.<br/>
        /// This parameter is only supported for `gpt-image-1`.
        /// </param>
        /// <param name="outputCompression">
        /// The compression level (0-100%) for the generated images.<br/>
        /// This parameter is only supported for the `webp` or `jpeg` output formats, and defaults to 100.<br/>
        /// This parameter is only supported for `gpt-image-1`.
        /// </param>
        /// <param name="size">
        /// The size of the generated images.
        /// Must be one of `1024x1024`, `1536x1024` (landscape), `1024x1536` (portrait), or `auto` (default value) for `gpt-image-1`,
        /// one of `256x256`, `512x512`, or `1024x1024` for `dall-e-2`, and one of `1024x1024`, `1792x1024`, or `1024x1792` for `dall-e-3`.
        /// </param>
        /// <param name="moderation">
        /// Control the content-moderation level for images generated by `gpt-image-1`. Must be either `low` for less restrictive filtering or `auto` (default value).<br/>
        /// This parameter is only supported for `gpt-image-1`.
        /// </param>
        /// <param name="background">
        /// Allows to set transparency for the background of the generated image(s). 
        /// Must be one of `transparent`, `opaque` or `auto` (default value).
        /// When `auto` is used, the model will automatically determine the best background for the image.
        /// If `transparent`, the output format needs to support transparency,
        /// so it should be set to either `png` (default value) or `webp`.<br/>
        /// This parameter is only supported for `gpt-image-1`.
        /// </param>
        /// <param name="style">
        /// The style of the generated images.
        /// Must be one of `vivid` or `natural`.
        /// Vivid causes the model to lean towards generating hyper-real and dramatic images.
        /// Natural causes the model to produce more natural, less hyper-real looking images.<br/>
        /// This parameter is only supported for `dall-e-3`.
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public ImageGenerationRequest(
            [JsonProperty("prompt")] string prompt,
            [JsonProperty("model")] Model model = null,
            [JsonProperty("n")] int? numberOfResults = null,
            [JsonProperty("quality")] string quality = null,
            [JsonProperty("response_format")] ImageResponseFormat responseFormat = 0,
            [JsonProperty("output_format")] string outputFormat = null,
            [JsonProperty("output_compression")] int? outputCompression = null,
            [JsonProperty("size")] string size = null,
            [JsonProperty("moderation")] string moderation = null,
            [JsonProperty("background")] string background = null,
            [JsonProperty("style")] string style = null,
            [JsonProperty("user")] string user = null)
        {
            Prompt = prompt;
            Model = string.IsNullOrWhiteSpace(model?.Id) ? Models.Model.DallE_2 : model;
            Number = numberOfResults;
            Quality = quality;
            ResponseFormat = responseFormat;
            OutputFormat = outputFormat;
            OutputCompression = outputCompression;
            Size = size;
            Moderation = moderation;
            Background = background;
            Style = style;
            User = user;

            if (!string.IsNullOrWhiteSpace(Style))
            {
                Model = Models.Model.DallE_3;
            }

            if (!string.IsNullOrWhiteSpace(OutputFormat) ||
                OutputCompression.HasValue ||
                !string.IsNullOrWhiteSpace(Moderation) ||
                !string.IsNullOrWhiteSpace(Background))
            {
                Model = Models.Model.GPT_Image_1;
            }
        }

        /// <summary>
        /// A text description of the desired image(s). The maximum length is 32000 characters for `gpt-image-1`, 1000 characters for `dall-e-2` and 4000 characters for `dall-e-3`.
        /// </summary>
        [Preserve]
        [JsonProperty("prompt")]
        [FunctionProperty("A text description of the desired image(s). " +
                          "The maximum length is 32000 characters for `gpt-image-1`, 1000 characters for `dall-e-2` and 4000 characters for `dall-e-3`.", true)]
        public string Prompt { get; }

        /// <summary>
        /// The model to use for image generation. One of `dall-e-2`, `dall-e-3`, or `gpt-image-1`.
        /// Defaults to `dall-e-2` unless a parameter specific to `gpt-image-1` is used.
        /// </summary>
        [Preserve]
        [JsonProperty("model", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The model to use for image generation. One of `dall-e-2`, `dall-e-3`, or `gpt-image-1`. " +
                          "Defaults to `dall-e-2` unless a parameter specific to `gpt-image-1` is used.",
            true,
            possibleValues: new object[] { "dall-e-2", "dall-e-3", "gpt-image-1" })]
        public string Model { get; }

        /// <summary>
        /// The number of images to generate.
        /// Must be between 1 and 10. For dall-e-3, only n=1 is supported.
        /// </summary>
        [Preserve]
        [JsonProperty("n", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The number of images to generate. Must be between 1 and 10. For dall-e-3, only n=1 is supported.", true, 1)]
        public int? Number { get; }

        /// <summary>
        /// The quality of the image that will be generated.<br/>
        /// - `auto` (default value) will automatically select the best quality for the given model.<br/>
        /// - `high`, `medium` and `low` are supported for `gpt-image-1`.<br/>
        /// - `hd` and `standard` are supported for `dall-e-3`.<br/>
        /// - `standard` is the only option for `dall-e-2`.
        /// </summary>
        [Preserve]
        [JsonProperty("quality", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The quality of the image that will be generated.\n" +
                          "- `auto` (default value) will automatically select the best quality for the given model.\n" +
                          "- `high`, `medium` and `low` are supported for `gpt-image-1`.\n" +
                          "- `hd` and `standard` are supported for `dall-e-3`.\n" +
                          "- `standard` is the only option for `dall-e-2`.",
            possibleValues: new object[] { "auto", "high", "hd", "standard" })]
        public string Quality { get; }

        /// <summary>
        /// The format in which generated images with `dall-e-2` and `dall-e-3`
        /// are returned. Must be one of `url` or `b64_json`. URLs are only
        /// valid for 60 minutes after the image has been generated.
        /// </summary>
        /// <remarks>
        /// `gpt-image-1` does not support urls and only supports base64-encoded images.
        /// </remarks>
        [Preserve]
        [JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The format in which generated images with `dall-e-2` and `dall-e-3` are returned. " +
                          "Must be one of `url` or `b64_json`. " +
                          "URLs are only valid for 60 minutes after the image has been generated. " +
                          "`gpt-image-1` does not support urls and only supports base64-encoded images.")]
        public ImageResponseFormat ResponseFormat { get; }

        /// <summary>
        /// The format in which the generated images are returned.
        /// Must be one of `png`, `jpeg`, or `webp`.
        /// </summary>
        /// <remarks>
        /// This parameter is only supported for `gpt-image-1`. 
        /// </remarks>
        [Preserve]
        [JsonProperty("output_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The format in which the generated images are returned. This parameter is only supported for `gpt-image-1`. Must be one of `png`, `jpeg`, or `webp`.",
            defaultValue: "png",
            possibleValues: new object[] { "png", "jpeg", "webp" })]
        public string OutputFormat { get; }

        /// <summary>
        /// The compression level (0-100%) for the generated images.
        /// This parameter is only supported the `webp` or `jpeg` output formats, and defaults to 100.
        /// </summary>
        /// <remarks>
        /// This parameter is only supported for `gpt-image-1`
        /// </remarks>
        [Preserve]
        [JsonProperty("output_compression", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The compression level (0-100%) for the generated images. " +
                          "This parameter is only supported for `gpt-image-1` with the `webp` or `jpeg` output formats, and defaults to 100.")]
        public int? OutputCompression { get; }

        /// <summary>
        /// The size of the generated images.
        /// Must be one of `1024x1024`, `1536x1024` (landscape), `1024x1536` (portrait), or `auto` (default value) for `gpt-image-1`,
        /// one of `256x256`, `512x512`, or `1024x1024` for `dall-e-2`, and one of `1024x1024`, `1792x1024`, or `1024x1792` for `dall-e-3`.
        /// </summary>
        [Preserve]
        [JsonProperty("size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The size of the generated images. " +
                          "Must be one of `1024x1024`, `1536x1024` (landscape), `1024x1536` (portrait), or `auto` (default value) for `gpt-image-1`, " +
                          "one of `256x256`, `512x512`, or `1024x1024` for `dall-e-2`, and one of `1024x1024`, `1792x1024`, or `1024x1792` for `dall-e-3`.",
            possibleValues: new object[] { "256x256", "512x512", "1024x1024", "1536x1024", "1024x1536", "1792x1024", "1024x1792", "auto" })]
        public string Size { get; }

        /// <summary>
        /// Control the content-moderation level for images generated by `gpt-image-1`. Must be either `low` for less restrictive filtering or `auto` (default value).
        /// </summary>
        /// <remarks>
        /// This parameter is only supported for `gpt-image-1`
        /// </remarks>
        [JsonProperty("moderation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("Control the content-moderation level for images generated by `gpt-image-1`. Must be either `low` for less restrictive filtering or `auto` (default value).",
            possibleValues: new object[] { "auto", "low" })]
        public string Moderation { get; }

        /// <summary>
        /// Allows to set transparency for the background of the generated image(s).
        /// Must be one of `transparent`, `opaque` or `auto` (default value).
        /// When `auto` is used, the model will automatically determine the best background for the image.
        /// If `transparent`, the output format needs to support transparency,
        /// so it should be set to either `png` (default value) or `webp`.
        /// </summary>
        /// <remarks>
        /// This parameter is only supported for `gpt-image-1`
        /// </remarks>
        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("Allows to set transparency for the background of the generated image(s). " +
                          "This parameter is only supported for `gpt-image-1`. " +
                          "Must be one of `transparent`, `opaque` or `auto` (default value). " +
                          "When `auto` is used, the model will automatically determine the best background for the image. " +
                          "If `transparent`, the output format needs to support transparency, so it should be set to either `png` (default value) or `webp`.",
            possibleValues: new object[] { "transparent", "opaque", "auto" })]
        public string Background { get; }

        /// <summary>
        /// The style of the generated images.
        /// Must be one of `vivid` or `natural`.
        /// Vivid causes the model to lean towards generating hyper-real and dramatic images.
        /// Natural causes the model to produce more natural, less hyper-real looking images.
        /// </summary>
        /// <remarks>
        /// This parameter is only supported for `dall-e-3`.
        /// </remarks>
        [Preserve]
        [JsonProperty("style", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("The style of the generated images. This parameter is only supported for `dall-e-3`. " +
                          "Must be one of `vivid` or `natural`. Vivid causes the model to lean towards generating hyper-real and dramatic images. " +
                          "Natural causes the model to produce more natural, less hyper-real looking images.",
            possibleValues: new object[] { "vivid", "natural" })]
        public string Style { get; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        [Preserve]
        [JsonProperty("user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.")]
        public string User { get; }
    }
}
