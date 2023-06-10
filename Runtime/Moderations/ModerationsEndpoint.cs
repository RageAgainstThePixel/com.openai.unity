// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities.WebRequestRest;

namespace OpenAI.Moderations
{
    /// <summary>
    /// The moderation endpoint is a tool you can use to check whether content complies with OpenAI's content policy.
    /// Developers can thus identify content that our content policy prohibits and take action, for instance by filtering it.
    /// <see href="https://platform.openai.com/docs/api-reference/moderations"/>
    /// </summary>
    public sealed class ModerationsEndpoint : OpenAIBaseEndpoint
    {
        /// <inheritdoc />
        public ModerationsEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "moderations";

        /// <summary>
        /// Classifies if text violates OpenAI's Content Policy.
        /// </summary>
        /// <param name="input">
        /// The input text to classify.
        /// </param>
        /// <param name="model">The default is text-moderation-latest which will be automatically upgraded over time.
        /// This ensures you are always using our most accurate model.
        /// If you use text-moderation-stable, we will provide advanced notice before updating the model.
        /// Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest.
        /// </param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>
        /// True, if the text has been flagged by the model as violating OpenAI's content policy.
        /// </returns>
        public async Task<bool> GetModerationAsync(string input, string model = null, CancellationToken cancellationToken = default)
        {
            var result = await CreateModerationAsync(new ModerationsRequest(input, model), cancellationToken);

            if (result?.Results == null ||
                result.Results.Count == 0)
            {
                return false;
            }

            return result.Results.Any(moderationResult => moderationResult.Flagged);
        }

        /// <summary>
        /// Classifies if text violates OpenAI's Content Policy
        /// </summary>
        /// <param name="request"><see cref="ModerationsRequest"/></param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        public async Task<ModerationsResponse> CreateModerationAsync(ModerationsRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, client.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl(), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate();
            return JsonConvert.DeserializeObject<ModerationsResponse>(response.Body, client.JsonSerializationOptions);
        }
    }
}
