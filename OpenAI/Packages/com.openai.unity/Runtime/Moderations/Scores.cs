// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Text;

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

        public override string ToString()
        {
            var sb = new StringBuilder()
                .AppendLine($"{"Hate:".PadRight(10)}{Hate:0.00 E+00}")
                .AppendLine($"{"Threat:".PadRight(10)}{HateThreatening:0.00 E+00}")
                .AppendLine($"{"Violence:".PadRight(10)}{Violence:0.00 E+00}")
                .AppendLine($"{"Graphic:".PadRight(10)}{ViolenceGraphic:0.00 E+00}")
                .AppendLine($"{"SelfHarm:".PadRight(10)}{SelfHarm:0.00 E+00}")
                .AppendLine($"{"Sexual:".PadRight(10)}{Sexual:0.00 E+00}")
                .AppendLine($"{"Minors:".PadRight(10)}{SexualMinors:0.00 E+00}");
            return sb.ToString();
        }

        public static implicit operator string(Scores scores) => scores.ToString();
    }
}
