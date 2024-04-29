// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Security.Authentication;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    [Serializable]
    public sealed class OpenAIAuthInfo : IAuthInfo
    {
        internal const string SecretKeyPrefix = "sk-";
        internal const string SessionKeyPrefix = "sess-";
        internal const string OrganizationPrefix = "org-";

        public OpenAIAuthInfo(string apiKey, string organizationId = null, string project = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidCredentialException(nameof(apiKey));
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

            if (!string.IsNullOrWhiteSpace(project))
            {
                this.projectId = project;
            }
        }

        [SerializeField]
        private string apiKey;

        /// <summary>
        /// The API key, required to access the service.
        /// </summary>
        public string ApiKey => apiKey;

        [SerializeField]
        [FormerlySerializedAs("organization")]
        private string organizationId;

        /// <summary>
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </summary>
        public string OrganizationId => organizationId;

        [SerializeField]
        private string projectId;
        public string ProjectId => projectId;
    }
}
