// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Security.Authentication;
using UnityEngine;

namespace OpenAI
{
    [Serializable]
    internal class AuthInfo
    {
        public AuthInfo(string apiKey, string organization = null)
        {
            if (!apiKey.Contains("sk-"))
            {
                throw new InvalidCredentialException($"{apiKey} must start with 'sk-'");
            }

            this.apiKey = apiKey;

            if (organization != null)
            {
                if (!organization.Contains("org-"))
                {
                    throw new InvalidCredentialException($"{nameof(organization)} must start with 'org-'");
                }

                this.organization = organization;
            }
        }

        [SerializeField]
        private string apiKey;

        public string ApiKey => apiKey;

        [SerializeField]
        private string organization;

        public string Organization => organization;
    }
}
