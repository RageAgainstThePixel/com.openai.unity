// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    public sealed class OpenAISettingsInfo : ISettingsInfo
    {
        internal const string Https = "https://";
        internal const string OpenAIDomain = "api.openai.com";
        internal const string DefaultOpenAIApiVersion = "v1";
        internal const string AzureOpenAIDomain = "openai.azure.com";
        internal const string DefaultAzureApiVersion = "2022-12-01";

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

            ResourceName = domain.Contains("http") ? domain : $"{Https}{domain}";
            ApiVersion = apiVersion;
            DeploymentId = string.Empty;
            BaseRequest = $"/{ApiVersion}/";
            BaseRequestUrlFormat = $"{ResourceName}{BaseRequest}{{0}}";
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
        /// Optional, defaults to 2022-12-01
        /// </param>
        /// <param name="useActiveDirectoryAuthentication">
        /// Optional, set to true if you want to use Azure Active Directory for Authentication.
        /// </param>
        public OpenAISettingsInfo(string resourceName, string deploymentId, string apiVersion = DefaultAzureApiVersion, bool useActiveDirectoryAuthentication = false)
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

            ResourceName = resourceName;
            DeploymentId = deploymentId;
            ApiVersion = apiVersion;
            BaseRequest = $"/openai/deployments/{DeploymentId}/";
            BaseRequestUrlFormat = $"{Https}{ResourceName}.{AzureOpenAIDomain}{BaseRequest}{{0}}?api-version={ApiVersion}";
            UseOAuthAuthentication = useActiveDirectoryAuthentication;
        }

        public string ResourceName { get; }

        public string ApiVersion { get; }

        public string DeploymentId { get; }

        public string BaseRequest { get; }

        internal string BaseRequestUrlFormat { get; }

        internal bool UseOAuthAuthentication { get; }

        public bool IsAzureDeployment => BaseRequestUrlFormat.Contains(AzureOpenAIDomain);
    }
}
