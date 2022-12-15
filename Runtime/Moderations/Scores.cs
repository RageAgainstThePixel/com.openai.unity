// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Scores
    {
        [JsonProperty("hate")]
        public double Hate { get; set; }

        [JsonProperty("hate/threatening")]
        public double HateThreatening { get; set; }

        [JsonProperty("self-harm")]
        public double SelfHarm { get; set; }

        [JsonProperty("sexual")]
        public double Sexual { get; set; }

        [JsonProperty("sexual/minors")]
        public double SexualMinors { get; set; }

        [JsonProperty("violence")]
        public double Violence { get; set; }

        [JsonProperty("violence/graphic")]
        public double ViolenceGraphic { get; set; }
    }
}
