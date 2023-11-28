// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Tests.Weather
{
    internal class WeatherArgs
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
