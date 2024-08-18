// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Utilities.WebRequestRest;

namespace OpenAI
{
    public abstract class OpenAIBaseEndpoint : BaseEndPoint<OpenAIClient, OpenAIAuthentication, OpenAISettings>
    {
        protected OpenAIBaseEndpoint(OpenAIClient client) : base(client) { }

        /// <summary>
        /// Indicates if the endpoint has an Azure Deployment.
        /// </summary>
        /// <remarks>
        /// If the endpoint is an Azure deployment, is true.
        /// If it is not an Azure deployment, is false.
        /// If it is not an Azure supported Endpoint, is null.
        /// </remarks>
        protected virtual bool? IsAzureDeployment => null;

        protected override string GetUrl(string endpoint = "", Dictionary<string, string> queryParameters = null)
        {
            string route;

            if (client.Settings.Info.IsAzureOpenAI && IsAzureDeployment == true)
            {
                route = $"{Root}deployments/{client.Settings.Info.DeploymentId}/{endpoint}";
            }
            else
            {
                route = $"{Root}{endpoint}";
            }

            var result = string.Format(client.Settings.Info.BaseRequestUrlFormat, route);

            foreach (var defaultQueryParameter in client.Settings.Info.DefaultQueryParameters)
            {
                queryParameters ??= new Dictionary<string, string>();
                queryParameters.Add(defaultQueryParameter.Key, defaultQueryParameter.Value);
            }

            if (queryParameters is { Count: not 0 })
            {
                result += $"?{string.Join('&', queryParameters.Select(parameter => $"{parameter.Key}={parameter.Value}"))}";
            }

            return result;
        }
    }
}
