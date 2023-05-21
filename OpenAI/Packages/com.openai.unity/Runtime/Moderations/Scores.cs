// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;

namespace OpenAI.Moderations
{
    public sealed class Scores
    {
        [JsonConstructor]
        public Scores(
            [JsonProperty("hate")] double hate,
            [JsonProperty("hate/threatening")] double hateThreatening,
            [JsonProperty("self-harm")] double selfHarm,
            [JsonProperty("sexual")] double sexual,
            [JsonProperty("sexual/minors")] double sexualMinors,
            [JsonProperty("violence")] double violence,
            [JsonProperty("violence/graphic")] double violenceGraphic)
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

        public override string ToString() =>
            $"{"Hate:",-10}{Hate:0.00 E+00}{Environment.NewLine}" +
            $"{"Threat:",-10}{HateThreatening:0.00 E+00}{Environment.NewLine}" +
            $"{"Violence:",-10}{Violence:0.00 E+00}{Environment.NewLine}" +
            $"{"Graphic:",-10}{ViolenceGraphic:0.00 E+00}{Environment.NewLine}" +
            $"{"SelfHarm:",-10}{SelfHarm:0.00 E+00}{Environment.NewLine}" +
            $"{"Sexual:",-10}{Sexual:0.00 E+00}{Environment.NewLine}" +
            $"{"Minors:",-10}{SexualMinors:0.00 E+00}{Environment.NewLine}";

        public static implicit operator string(Scores scores) => scores.ToString();
    }
}
