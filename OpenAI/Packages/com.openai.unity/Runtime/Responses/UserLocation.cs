// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class UserLocation
    {
        [Preserve]
        public UserLocation(string city = null, string country = null, string region = null, string timezone = null)
        {
            City = city;
            Country = country;
            Region = region;
            Timezone = timezone;
        }

        [Preserve]
        [JsonConstructor]
        internal UserLocation(
            [JsonProperty("type")] string type,
            [JsonProperty("city")] string city,
            [JsonProperty("country")] string country,
            [JsonProperty("region")] string region,
            [JsonProperty("timezone")] string timezone)
        {
            Type = type;
            City = city;
            Country = country;
            Region = region;
            Timezone = timezone;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; } = "approximate";

        /// <summary>
        /// Free text input for the city of the user, e.g. San Francisco.
        /// </summary>
        [Preserve]
        [JsonProperty("city", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string City { get; private set; }

        /// <summary>
        /// The two-letter ISO country code of the user, e.g. US.
        /// </summary>
        [Preserve]
        [JsonProperty("country", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Country { get; private set; }

        /// <summary>
        /// Free text input for the region of the user, e.g. California.
        /// </summary>
        [Preserve]
        [JsonProperty("region", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Region { get; private set; }

        /// <summary>
        /// The IANA timezone of the user, e.g. America/Los_Angeles.
        /// </summary>
        [Preserve]
        [JsonProperty("timezone", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Timezone { get; private set; }
    }
}
