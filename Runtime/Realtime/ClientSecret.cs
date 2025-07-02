// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ClientSecret
    {
        [Preserve]
        public ClientSecret(int? expiresAfter = null)
        {
            ExpiresAfter = expiresAfter ?? 600;
        }

        [Preserve]
        [JsonConstructor]
        internal ClientSecret(
            [JsonProperty("value")] string ephemeralApiKey,
            [JsonProperty("expires_at")] int? expiresAtUnixTimeSeconds = null,
            [JsonProperty("expires_after")] ExpiresAfter expiresAfter = null)
        {
            EphemeralApiKey = ephemeralApiKey;
            ExpiresAtUnixTimeSeconds = expiresAtUnixTimeSeconds;
            ExpiresAfter = expiresAfter;
        }

        [Preserve]
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EphemeralApiKey { get; }

        [Preserve]
        [JsonProperty("expires_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ExpiresAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime? ExpiresAt => ExpiresAtUnixTimeSeconds.HasValue
            ? DateTimeOffset.FromUnixTimeSeconds(ExpiresAtUnixTimeSeconds.Value).UtcDateTime
            : null;

        [Preserve]
        [JsonProperty("expires_after", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ExpiresAfter ExpiresAfter { get; }

        [Preserve]
        public static implicit operator string(ClientSecret clientSecret) => clientSecret?.EphemeralApiKey;
    }
}
