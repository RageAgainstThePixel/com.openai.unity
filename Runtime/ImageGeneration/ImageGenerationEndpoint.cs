// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Images
{
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    public class ImageGenerationEndpoint : BaseEndPoint
    {
        /// <inheritdoc />
        internal ImageGenerationEndpoint(OpenAIClient api) : base(api) { }

        /// <inheritdoc />
        protected override string GetEndpoint(Engine engine = null) => $"{Api.BaseUrl}images/generations";

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="numberOfResults"></param>
        /// <param name="size"></param>
        /// <returns>An array of generated textures.</returns>
        public async Task<IReadOnlyList<Texture2D>> GenerateImageAsync(string prompt, int numberOfResults = 1, ImageSize size = ImageSize.Large)
            => await GenerateImageAsync(new ImageGenerationRequest(prompt, numberOfResults, size));

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="request"><see cref="ImageGenerationRequest"/></param>
        /// <returns>An array of generated textures.</returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IReadOnlyList<Texture2D>> GenerateImageAsync(ImageGenerationRequest request)
        {
            var jsonContent = JsonConvert.SerializeObject(request, Api.JsonSerializationOptions);
            var response = await Api.Client.PostAsync(GetEndpoint(), jsonContent.ToJsonStringContent());

            if (response.IsSuccessStatusCode)
            {
                var resultAsString = await response.Content.ReadAsStringAsync();
                var imageGenerationResponse = JsonConvert.DeserializeObject<ImageGenerationResponse>(resultAsString, Api.JsonSerializationOptions);

                if (imageGenerationResponse?.Data == null || imageGenerationResponse.Data.Count == 0)
                {
                    throw new HttpRequestException($"{nameof(GenerateImageAsync)} returned no results!  HTTP status code: {response.StatusCode}. Response body: {resultAsString}");
                }

                imageGenerationResponse.SetResponseData(response.Headers);

                var images = new List<Texture2D>(imageGenerationResponse.Data.Count);

                foreach (var imageResult in imageGenerationResponse.Data)
                {
                    images.Add(await Rest.DownloadTextureAsync(imageResult.Url));
                }
                
                return images;
            }

            throw new HttpRequestException($"{nameof(GenerateImageAsync)} Failed!  HTTP status code: {response.StatusCode}. Request body: {jsonContent}");
        }
    }
}
