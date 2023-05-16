// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using UnityEngine;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    public sealed class OpenAISettings : ISettings<OpenAISettingsInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> for use with OpenAI.
        /// </summary>
        public OpenAISettings()
        {
            if (cachedDefault != null) { return; }

            var config = Resources.LoadAll<OpenAIConfiguration>(string.Empty)
                .FirstOrDefault(asset => asset != null);

            if (config != null)
            {
                if (config.UseAzureOpenAI)
                {
                    Default = new OpenAISettings(new OpenAISettingsInfo(
                        config.ResourceName,
                        config.DeploymentId,
                        config.ApiVersion,
                        config.UseAzureActiveDirectory));
                }
                else
                {
                    Default = new OpenAISettings(new OpenAISettingsInfo(
                        domain: config.ProxyDomain,
                        apiVersion: config.ApiVersion));
                }
            }
            else
            {
                Default = new OpenAISettings(new OpenAISettingsInfo());
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> with the provided <see cref="OpenAISettingsInfo"/>.
        /// </summary>
        /// <param name="settingsInfo"></param>
        public OpenAISettings(OpenAISettingsInfo settingsInfo)
            => this.settingsInfo = settingsInfo;

        /// <summary>
        /// Creates a new instance of <see cref="OpenAISettings"/> for use with OpenAI.
        /// </summary>
        /// <param name="domain">Base api domain.</param>
        /// <param name="apiVersion">The version of the OpenAI api you want to use.</param>
        public OpenAISettings(string domain, string apiVersion = OpenAISettingsInfo.DefaultOpenAIApiVersion)
            => settingsInfo = new OpenAISettingsInfo(domain, apiVersion);

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
        /// Optional, defaults to 2022-12-01
        /// </param>
        /// <param name="useActiveDirectoryAuthentication">
        /// Optional, set to true if you want to use Azure Active Directory for Authentication.
        /// </param>
        public OpenAISettings(string resourceName, string deploymentId, string apiVersion = OpenAISettingsInfo.DefaultAzureApiVersion, bool useActiveDirectoryAuthentication = false)
            => settingsInfo = new OpenAISettingsInfo(resourceName, deploymentId, apiVersion, useActiveDirectoryAuthentication);

        [Obsolete("Obsolete")]
        internal OpenAISettings(OpenAIClientSettings clientSettings) => settingsInfo = new OpenAISettingsInfo(clientSettings);

        private static OpenAISettings cachedDefault;

        public static OpenAISettings Default
        {
            get => cachedDefault ?? new OpenAISettings();
            internal set => cachedDefault = value;
        }

        private readonly OpenAISettingsInfo settingsInfo;

        public OpenAISettingsInfo Info => settingsInfo ?? Default.Info;

        public string BaseRequestUrlFormat => Info.BaseRequestUrlFormat;
    }
}
