// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    public sealed class OpenAISettingsInfo : ISettingsInfo
    {
        internal const string WSS = "wss://";
        internal const string Http = "http://";
        internal const string Https = "https://";
        internal const string OpenAIDomain = "api.openai.com";
        internal const string DefaultOpenAIApiVersion = "v1";
        internal const string AzureOpenAIDomain = "openai.azure.com";
        internal const string DefaultAzureApiVersion = "2024-10-21";

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettingsInfo"/> for use with OpenAI.
        /// </summary>
        public OpenAISettingsInfo()
        {
            ResourceName = OpenAIDomain;
            ApiVersion = DefaultOpenAIApiVersion;
            DeploymentId = string.Empty;
            BaseRequest = $"/{ApiVersion}/";
            BaseRequestUrlFormat = $"{Https}{ResourceName}{BaseRequest}{{0}}";
            BaseWebSocketUrlFormat = $"{WSS}{ResourceName}{BaseRequest}{{0}}";
            UseOAuthAuthentication = true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettingsInfo"/> for use with OpenAI.
        /// </summary>
        /// <param name="domain">Base api domain.</param>
        /// <param name="apiVersion">The version of the OpenAI api you want to use.</param>
        public OpenAISettingsInfo(string domain, string apiVersion = DefaultOpenAIApiVersion)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                domain = OpenAIDomain;
            }

            if (!domain.Contains('.') &&
                !domain.Contains(':'))
            {
                throw new ArgumentException($"You're attempting to pass a \"resourceName\" parameter to \"{nameof(domain)}\". Please specify \"resourceName:\" for this parameter in constructor.");
            }

            if (string.IsNullOrWhiteSpace(apiVersion))
            {
                apiVersion = DefaultOpenAIApiVersion;
            }

            var protocol = Https;

            if (domain.StartsWith(Http))
            {
                protocol = Http;
                domain = domain.Replace(Http, string.Empty);
            }
            else if (domain.StartsWith(Https))
            {
                protocol = Https;
                domain = domain.Replace(Https, string.Empty);
            }

            ResourceName = $"{protocol}{domain}";
            ApiVersion = apiVersion;
            DeploymentId = string.Empty;
            BaseRequest = $"/{ApiVersion}/";
            BaseRequestUrlFormat = $"{ResourceName}{BaseRequest}{{0}}";
            BaseWebSocketUrlFormat = $"{WSS}{OpenAIDomain}{BaseRequest}{{0}}";
            UseOAuthAuthentication = true;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OpenAISettingsInfo"/> for use with Azure OpenAI.<br/>
        /// <see href="https://learn.microsoft.com/en-us/azure/cognitive-services/openai/"/>
        /// </summary>
        /// <param name="resourceName">
        /// The name of your Azure OpenAI Resource.
        /// </param>
        /// <param name="deploymentId">
        /// The name of your model deployment. You're required to first deploy a model before you can make calls.
        /// </param>
        /// <param name="apiVersion">
        /// Optional, defaults to 2024-10-21.
        /// </param>
        /// <param name="useActiveDirectoryAuthentication">
        /// Optional, set to true if you want to use Azure Active Directory for Authentication.
        /// </param>
        /// Optional, override the azure domain, if you need to use a different one (e.g., for Azure Government or other regions).
        /// <param name="azureDomain">
        /// Optional, defaults to "openai.azure.com".
        /// </param>
        public OpenAISettingsInfo(
            string resourceName,
            string deploymentId,
            string apiVersion = DefaultAzureApiVersion,
            bool useActiveDirectoryAuthentication = false,
            string azureDomain = AzureOpenAIDomain)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (resourceName.Contains('.') ||
                resourceName.Contains(':'))
            {
                throw new ArgumentException($"You're attempting to pass a \"domain\" parameter to \"{nameof(resourceName)}\". Please specify \"domain:\" for this parameter in constructor.");
            }

            if (string.IsNullOrWhiteSpace(apiVersion))
            {
                apiVersion = DefaultAzureApiVersion;
            }

            if (string.IsNullOrWhiteSpace(azureDomain))
            {
                azureDomain = AzureOpenAIDomain;
            }

            IsAzureOpenAI = true;
            ResourceName = resourceName;
            DeploymentId = deploymentId;
            ApiVersion = apiVersion;
            BaseRequest = "/openai/";
            BaseRequestUrlFormat = $"{Https}{ResourceName}.{azureDomain}{BaseRequest}{{0}}";
            BaseWebSocketUrlFormat = $"{WSS}{ResourceName}.{azureDomain}{BaseRequest}{{0}}";
            defaultQueryParameters.Add("api-version", ApiVersion);
            UseOAuthAuthentication = useActiveDirectoryAuthentication;
        }

        public string ResourceName { get; }

        public string DeploymentId { get; }

        public string ApiVersion { get; }

        public string BaseRequest { get; }

        internal string BaseRequestUrlFormat { get; }

        internal string BaseWebSocketUrlFormat { get; }

        internal bool UseOAuthAuthentication { get; }

        public bool IsAzureOpenAI { get; }

        private readonly Dictionary<string, string> defaultQueryParameters = new();

        internal IReadOnlyDictionary<string, string> DefaultQueryParameters => defaultQueryParameters;
    }
}
