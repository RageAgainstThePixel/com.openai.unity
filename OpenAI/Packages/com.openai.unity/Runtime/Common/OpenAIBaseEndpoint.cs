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

        /// <summary>
        /// Indicates if the endpoint is for a WebSocket.
        /// </summary>
        protected virtual bool? IsWebSocketEndpoint => null;

        /// <summary>
        /// Gets the full formatted url for the API endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint url.</param>
        /// <param name="queryParameters">Optional, parameters to add to the endpoint.</param>
        protected override string GetUrl(string endpoint = "", Dictionary<string, string> queryParameters = null)
        {
            string route;

            if (client.Settings.Info.IsAzureOpenAI && IsAzureDeployment == true)
            {
                route = $"deployments/{client.Settings.Info.DeploymentId}/{Root}{endpoint}";
            }
            else
            {
                route = $"{Root}{endpoint}";
            }

            var baseUrlFormat = IsWebSocketEndpoint == true
                ? client.Settings.Info.BaseWebSocketUrlFormat
                : client.Settings.Info.BaseRequestUrlFormat;
            var url = string.Format(baseUrlFormat, route);

            foreach (var defaultQueryParameter in client.Settings.Info.DefaultQueryParameters)
            {
                queryParameters ??= new Dictionary<string, string>();
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
