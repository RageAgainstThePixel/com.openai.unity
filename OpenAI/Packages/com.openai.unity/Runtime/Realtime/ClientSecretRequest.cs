// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    internal sealed class ClientSecretRequest
    {
        [Preserve]
        public ClientSecretRequest(SessionConfiguration session, ExpiresAfter expiresAfter)
        {
            Session = session;
            ExpiresAfter = expiresAfter;
        }

        [Preserve]
        [JsonProperty("expires_after", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ExpiresAfter ExpiresAfter { get; }

        [Preserve]
        [JsonProperty("session", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SessionConfiguration Session { get; }
    }
}
