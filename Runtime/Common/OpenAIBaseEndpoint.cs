// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
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

        /// <summary>
        /// Gets the full formatted url for the API endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint url.</param>
        /// <param name="queryParameters">Optional, parameters to add to the endpoint.</param>
        protected override string GetUrl(string endpoint = "", Dictionary<string, string> queryParameters = null)
            => GetEndpoint(client.Settings.Info.BaseRequestUrlFormat, endpoint, queryParameters);

        protected string GetWebsocketUri(string endpoint = "", Dictionary<string, string> queryParameters = null)
            => GetEndpoint(client.Settings.Info.BaseWebSocketUrlFormat, endpoint, queryParameters);

        private string GetEndpoint(string baseUrlFormat, string endpoint = "", Dictionary<string, string> queryParameters = null)
        {
            string route;

            if (client.Settings.Info.IsAzureOpenAI && IsAzureDeployment == true)
            {
                if (string.IsNullOrWhiteSpace(client.Settings.Info.DeploymentId))
                {
                    throw new InvalidOperationException("Deployment ID must be provided for Azure OpenAI endpoints.");
                }

                route = $"deployments/{client.Settings.Info.DeploymentId}/{Root}{endpoint}";
            }
            else
            {
                route = $"{Root}{endpoint}";
            }

            var url = string.Format(baseUrlFormat, route);

            foreach (var defaultQueryParameter in client.Settings.Info.DefaultQueryParameters)
            {
                queryParameters ??= new();
                queryParameters.Add(defaultQueryParameter.Key, defaultQueryParameter.Value);
            }

            if (queryParameters is { Count: not 0 })
            {
                url += $"?{string.Join('&', queryParameters.Select(parameter => $"{parameter.Key}={parameter.Value}"))}";
            }

            return url;
        }
    }
}
