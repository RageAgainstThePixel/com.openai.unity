// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenAI
{
    /// <summary>
    /// Classifies the specified query using provided examples.
    /// The endpoint first searches over the labeled examples to select the ones most relevant for the particular query.
    /// Then, the relevant examples are combined with the query to construct a prompt to produce the final label via the completions endpoint.
    /// <see href="https://beta.openai.com/docs/guides/classifications"/>
    /// </summary>
    public class ClassificationEndpoint : BaseEndPoint
    {
        /// <inheritdoc />
        internal ClassificationEndpoint(OpenAIClient api) : base(api) { }

        /// <inheritdoc />
        protected override string GetEndpoint(Engine engine = null) => $"{Api.BaseUrl}classifications";

        /// <summary>
        /// Given a query and a set of labeled examples, the model will predict the most likely label for the query.
        /// </summary>
        /// <param name="request">The <see cref="ClassificationRequest"/> to use for the query.</param>
        /// <returns>A <see cref="ClassificationResponse"/>.</returns>
        public async Task<ClassificationResponse> GetClassificationAsync(ClassificationRequest request)
        {
            var jsonContent = JsonConvert.SerializeObject(request, Api.JsonSerializationOptions);
            var response = await Api.Client.PostAsync(GetEndpoint(), jsonContent.ToJsonStringContent());

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ClassificationResponse>(await response.Content.ReadAsStringAsync());
                result.SetResponseData(response.Headers);
                return result;
            }

            throw new HttpRequestException($"{nameof(GetClassificationAsync)} Failed! HTTP status code: {response.StatusCode}. Request body: {jsonContent}");
        }
    }
}
