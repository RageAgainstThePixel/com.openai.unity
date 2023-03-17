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
        internal const string SecretKeyPrefix = "sk-";
        internal const string SessionKeyPrefix = "sess-";
        internal const string OrganizationPrefix = "org-";

        public AuthInfo(string apiKey, string organizationId = null)
        {
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
