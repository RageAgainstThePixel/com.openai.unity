// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;

namespace OpenAI.Images
{
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    public sealed class ImageGenerationRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prompt">
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </param>
        /// <param name="numberOfResults">
        /// The number of images to generate. Must be between 1 and 10.
        /// </param>
        /// <param name="size">
        /// The size of the generated images.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ImageGenerationRequest(string prompt, int numberOfResults, ImageSize size)
        {
            if (prompt.Length > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(prompt), "The maximum character length for the prompt is 1000 characters.");
            }

            Prompt = prompt;

            if (numberOfResults is > 10 or < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            }

            Number = numberOfResults;

            Size = size switch
            {
                ImageSize.Small => "256x256",
                ImageSize.Medium => "512x512",
                ImageSize.Large => "1024x1024",
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };
        }

        /// <summary>
        /// A text description of the desired image(s). The maximum length is 1000 characters.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt { get; }

        /// <summary>
        /// The number of images to generate. Must be between 1 and 10.
        /// </summary>
        [JsonProperty("n")]
        public int Number { get; }

        /// <summary>
        /// The size of the generated images.
        /// </summary>
        [JsonProperty("size")]
        public string Size { get; }
    }
}
