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
        private const string SecretKeyPrefix = "sk-";
        private const string SessionKeyPrefix = "sess-";
        private const string OrganizationPrefix = "org-";

        public AuthInfo(string apiKey, string organizationId = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey) ||
                (!apiKey.Contains(SecretKeyPrefix) &&
                 !apiKey.Contains(SessionKeyPrefix)))
            {
                throw new InvalidCredentialException($"{apiKey} must start with '{SecretKeyPrefix}'");
            }

            this.apiKey = apiKey;

            if (!string.IsNullOrWhiteSpace(organizationId))
            {
                if (!organizationId.Contains(OrganizationPrefix))
                {
                    throw new InvalidCredentialException($"{nameof(organizationId)} must start with '{OrganizationPrefix}'");
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
