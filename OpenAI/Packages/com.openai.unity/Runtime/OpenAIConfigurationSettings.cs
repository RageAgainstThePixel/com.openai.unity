// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Serialization;

namespace OpenAI
{
    [CreateAssetMenu(fileName = nameof(OpenAIConfigurationSettings), menuName = "OpenAI/" + nameof(OpenAIConfigurationSettings), order = 0)]
    internal class OpenAIConfigurationSettings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The OpenAI api key.")]
        internal string apiKey;

        /// <summary>
        /// The OpenAI api key.
        /// </summary>
        public string ApiKey => apiKey;

        [SerializeField]
        [FormerlySerializedAs("organization")]
        [Tooltip("For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.\n\n" +
                 "Usage from these API requests will count against the specified organization's subscription quota.")]
        internal string organizationId;

        /// <summary>
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </summary>
        public string OrganizationId => organizationId;
    }
}
