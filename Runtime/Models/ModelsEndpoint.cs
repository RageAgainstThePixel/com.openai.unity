// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using Utilities.WebRequestRest;

namespace OpenAI.Models
{
    /// <summary>
    /// List and describe the various models available in the API.
    /// You can refer to the Models documentation to understand which models are available for certain endpoints: <see href="https://platform.openai.com/docs/models/model-endpoint-compatibility"/>.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/models"/>
    /// </summary>
    public sealed class ModelsEndpoint : OpenAIBaseEndpoint
    {
        [Preserve]
        private class ModelsList
        {
            [JsonProperty("data")]
            public List<Model> Data { get; set; }
        }

        [Preserve]
        private class DeleteModelResponse
        {
            [JsonConstructor]
            public DeleteModelResponse(
                [JsonProperty("id")] string id,
                [JsonProperty("object")] string @object,
                [JsonProperty("deleted")] bool deleted)
            {
                Id = id;
                Object = @object;
                Deleted = deleted;
            }

            [JsonProperty("id")]
            public string Id { get; }

            [JsonProperty("object")]
            public string Object { get; }

            [JsonProperty("deleted")]
            public bool Deleted { get; }
        }

        /// <inheritdoc />
        public ModelsEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "models";

        /// <summary>
        /// List all models via the API
        /// </summary>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>Asynchronously returns the list of all <see cref="Model"/>s</returns>
        public async Task<IReadOnlyList<Model>> GetModelsAsync(CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl(), new RestParameters(client.DefaultRequestHeaders), cancellationToken: cancellationToken);
            response.Validate();
            return JsonConvert.DeserializeObject<ModelsList>(response.Body, client.JsonSerializationOptions)?.Data;
        }

        /// <summary>
        /// Get the details about a particular Model from the API
        /// </summary>
        /// <param name="id">The id/name of the model to get more details about</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>Asynchronously returns the <see cref="Model"/> with all available properties</returns>
        public async Task<Model> GetModelDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            var response = await Rest.GetAsync(GetUrl($"/{id}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken: cancellationToken);
            response.Validate();
            return JsonConvert.DeserializeObject<Model>(response.Body, client.JsonSerializationOptions);
        }

        /// <summary>
        /// Delete a fine-tuned model. You must have the Owner role in your organization.
        /// </summary>
        /// <param name="modelId">The <see cref="Model"/> to delete.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>True, if fine-tuned model was successfully deleted.</returns>
        public async Task<bool> DeleteFineTuneModelAsync(string modelId, CancellationToken cancellationToken = default)
        {
            var model = await GetModelDetailsAsync(modelId, cancellationToken);

            if (model == null ||
                string.IsNullOrWhiteSpace(model))
            {
                throw new Exception($"Failed to get {modelId} info!");
            }

            // Don't check ownership as API does it for us.

            try
            {
                var response = await Rest.DeleteAsync(GetUrl($"/{model.Id}"), new RestParameters(client.DefaultRequestHeaders), cancellationToken: cancellationToken);
                response.Validate();
                return JsonConvert.DeserializeObject<DeleteModelResponse>(response.Body, client.JsonSerializationOptions)?.Deleted ?? false;
            }
            catch (RestException e)
            {
                if (e.Response.Code == 403 ||
                    e.Message.Contains("You have insufficient permissions for this operation. You need to be this role: Owner."))
                {
                    throw new UnauthorizedAccessException("You have insufficient permissions for this operation. You need to be this role: Owner.");
                }

                throw;
            }
        }
    }
}
