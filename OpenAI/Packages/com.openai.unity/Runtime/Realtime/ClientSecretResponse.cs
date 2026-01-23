// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ClientSecretResponse
    {
        [Preserve]
        [JsonConstructor]
        internal ClientSecretResponse(
            [JsonProperty("value")] string value,
            [JsonProperty("expires_at")] int? expiresAtUnixTimeSeconds,
            [JsonProperty("session")] SessionConfiguration session)
        {
            Value = value;
            ExpiresAtUnixTimeSeconds = expiresAtUnixTimeSeconds;
            Session = session;
        }

        /// <summary>
        /// The ephemeral client secret value.
        /// </summary>
        [Preserve]
        [JsonProperty("value")]
        public string Value { get; }

        /// <summary>
        /// Expiration timestamp in seconds since epoch.
        /// </summary>
        [Preserve]
        [JsonProperty("expires_at")]
        public int? ExpiresAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? ExpiresAt => ExpiresAtUnixTimeSeconds.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixTimeSeconds.Value).UtcDateTime
            : null;

        /// <summary>
        /// The effective session configuration associated with the secret.
        /// </summary>
        [Preserve]
        [JsonProperty("session")]
        public SessionConfiguration Session { get; }
    }
}
