// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Scores
    {
        [JsonConstructor]
        public Scores(double hate, double hateThreatening, double selfHarm, double sexual, double sexualMinors, double violence, double violenceGraphic)
        {
            Hate = hate;
            HateThreatening = hateThreatening;
            SelfHarm = selfHarm;
            Sexual = sexual;
            SexualMinors = sexualMinors;
            Violence = violence;
            ViolenceGraphic = violenceGraphic;
        }

        [JsonProperty("hate")]
        public double Hate { get; }

        [JsonProperty("hate/threatening")]
        public double HateThreatening { get; }

        [JsonProperty("self-harm")]
        public double SelfHarm { get; }

        [JsonProperty("sexual")]
        public double Sexual { get; }

        [JsonProperty("sexual/minors")]
        public double SexualMinors { get; }

        [JsonProperty("violence")]
        public double Violence { get; }

        [JsonProperty("violence/graphic")]
        public double ViolenceGraphic { get; }
    }
}
