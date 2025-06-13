// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using UnityEngine;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    public sealed class OpenAISettings : ISettings<OpenAISettingsInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> with default <see cref="OpenAISettingsInfo"/>.
        /// </summary>
        public OpenAISettings()
        {
            Info = new OpenAISettingsInfo();
            cachedDefault = this;
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> with provided <see cref="configuration"/>.
        /// </summary>
        /// <param name="configuration"><see cref="OpenAIConfiguration"/>.</param>
        public OpenAISettings(OpenAIConfiguration configuration)
        {
            if (configuration == null)
            {
                Debug.LogWarning($"You can speed this up by passing a {nameof(OpenAIConfiguration)} to the {nameof(OpenAISettings)}.ctr");
                configuration = Resources.LoadAll<OpenAIConfiguration>(string.Empty).FirstOrDefault(asset => asset != null);
            }

            if (configuration != null)
            {
                Info = configuration.UseAzureOpenAI
                    ? new OpenAISettingsInfo(
                        resourceName: configuration.ResourceName,
                        deploymentId: configuration.DeploymentId,
                        apiVersion: configuration.ApiVersion,
                        useActiveDirectoryAuthentication: configuration.UseAzureActiveDirectory)
                    : new OpenAISettingsInfo(
                        domain: configuration.ProxyDomain,
                        apiVersion: configuration.ApiVersion);
            }
            else
            {
                Info = new OpenAISettingsInfo();
            }

            cachedDefault = this;
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> with the provided <see cref="settingsInfo"/>.
        /// </summary>
        /// <param name="settingsInfo"><see cref="OpenAISettingsInfo"/>.</param>
        public OpenAISettings(OpenAISettingsInfo settingsInfo)
        {
            Info = settingsInfo;
            cachedDefault = this;
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/>.
        /// </summary>
        /// <param name="domain">Base api domain.</param>
        /// <param name="apiVersion">The version of the OpenAI api you want to use.</param>
        public OpenAISettings(string domain, string apiVersion = OpenAISettingsInfo.DefaultOpenAIApiVersion)
        {
            Info = new OpenAISettingsInfo(domain, apiVersion);
            cachedDefault = this;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OpenAISettings"/> for use with Azure OpenAI.<br/>
        /// <see href="https://learn.microsoft.com/en-us/azure/cognitive-services/openai/"/>
        /// </summary>
        /// <param name="resourceName">
        /// The name of your Azure OpenAI Resource.
        /// </param>
        /// <param name="deploymentId">
        /// The name of your model deployment. You're required to first deploy a model before you can make calls.
        /// </param>
        /// <param name="apiVersion">
        /// Optional, defaults to 2024-10-21
        /// </param>
        /// <param name="useActiveDirectoryAuthentication">
        /// Optional, set to true if you want to use Azure Active Directory for Authentication.
        /// </param>
        /// <param name="azureDomain">
        /// Optional, defaults to "openai.azure.com".
        /// </param>
        public OpenAISettings(
            string resourceName,
            string deploymentId,
            string apiVersion = OpenAISettingsInfo.DefaultAzureApiVersion,
            bool useActiveDirectoryAuthentication = false,
            string azureDomain = OpenAISettingsInfo.AzureOpenAIDomain)
        {
            Info = new OpenAISettingsInfo(resourceName, deploymentId, apiVersion, useActiveDirectoryAuthentication, azureDomain);
            cachedDefault = this;
        }

        private static OpenAISettings cachedDefault;

        public static OpenAISettings Default
        {
            get => cachedDefault ??= new OpenAISettings(configuration: null);
            internal set => cachedDefault = value;
        }

        public OpenAISettingsInfo Info { get; }

        public string BaseRequestUrlFormat => Info.BaseRequestUrlFormat;
    }
}
