// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

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
        [Tooltip("For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request. Usage from these API requests will count against the specified organization's subscription quota.")]
        internal string organization;

        /// <summary>
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </summary>
        public string Organization => organization;
    }
}
