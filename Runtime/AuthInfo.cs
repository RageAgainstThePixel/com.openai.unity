// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Security.Authentication;
using UnityEngine;
using UnityEngine.Serialization;

namespace OpenAI
{
    [Serializable]
    internal class AuthInfo
    {
        public AuthInfo(string apiKey, string organizationId = null)
        {
            if (!apiKey.Contains("sk-"))
            {
                throw new InvalidCredentialException($"{apiKey} must start with 'sk-'");
            }

            this.apiKey = apiKey;

            if (organizationId != null)
            {
                if (!organizationId.Contains("org-"))
                {
                    throw new InvalidCredentialException($"{nameof(organizationId)} must start with 'org-'");
                }

                this.organizationId = organizationId;
            }
        }

        [SerializeField]
        private string apiKey;

        public string ApiKey => apiKey;

        [SerializeField]
        [FormerlySerializedAs("organization")]
        private string organizationId;

        public string OrganizationId => organizationId;
    }
}
