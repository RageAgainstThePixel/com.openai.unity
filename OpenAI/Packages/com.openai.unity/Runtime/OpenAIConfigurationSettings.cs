// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace OpenAI
{
    [CreateAssetMenu(fileName = nameof(OpenAIConfigurationSettings), menuName = "OpenAI/" + nameof(OpenAIConfigurationSettings))]
    internal class OpenAIConfigurationSettings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The OpenAI api key.")]
        private string apiKey;

        /// <summary>
        /// The OpenAI api key.
        /// </summary>
        public string ApiKey => apiKey;
    }
}
