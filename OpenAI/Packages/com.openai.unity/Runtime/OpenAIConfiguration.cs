// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Serialization;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    [CreateAssetMenu(fileName = nameof(OpenAIConfiguration), menuName = "OpenAI/" + nameof(OpenAIConfiguration), order = 0)]
    public class OpenAIConfiguration : ScriptableObject, IConfiguration
    {
        [SerializeField]
        [Tooltip("The OpenAI or Azure api key.")]
        internal string apiKey;

        /// <summary>
        /// The OpenAI api key.
        /// </summary>
        public string ApiKey
        {
            get => apiKey;
            internal set => apiKey = value;
        }

        [SerializeField]
        [FormerlySerializedAs("organization")]
        [Tooltip("For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.\n\n" +
                 "Usage from these API requests will count against the specified organization's subscription quota.")]
        internal string organizationId;

        /// <summary>
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </summary>
        public string OrganizationId
        {
            get => organizationId;
            internal set => organizationId = value;
        }

        [SerializeField]
        [Tooltip("Check this box if you're using OpenAI on Azure.")]
        private bool useAzureOpenAI;

        public bool UseAzureOpenAI => useAzureOpenAI;

        [SerializeField]
        [Tooltip("Optional proxy domain to make requests though.")]
        private string proxyDomain;

        public string ProxyDomain => proxyDomain;

        [SerializeField]
        [Tooltip("The name of your Azure OpenAI Resource.")]
        private string resourceName;

        public string ResourceName => resourceName;

        [SerializeField]
        [Tooltip("The name of your model deployment. You're required to first deploy a model before you can make calls.")]
        private string deploymentId;

        public string DeploymentId => deploymentId;

        [SerializeField]
        [Tooltip("Authenticate an API call using an Azure Active Directory token.")]
        private bool useAzureActiveDirectory;

        public bool UseAzureActiveDirectory => useAzureActiveDirectory;

        [SerializeField]
        [Tooltip("The api version, Defaults to v1 for OpenAI, and 2022-12-01 for Azure")]
        private string apiVersion;

        public string ApiVersion => apiVersion;
    }
}
