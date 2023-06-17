// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Tests.Weather
{
    internal class WeatherService
    {
        public static string GetCurrentWeather(WeatherArgs weatherArgs)
        {
            return $"The current weather in {weatherArgs.Location} is 20 {weatherArgs.Unit}";
        }
    }

    internal class WeatherArgs
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
