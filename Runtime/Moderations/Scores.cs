// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Scores
    {
        [JsonConstructor]
        public Scores(
            [JsonProperty("hate")] double hate,
            [JsonProperty("hate/threatening")] double hateThreatening,
            [JsonProperty("harassment")] double harassment,
            [JsonProperty("harassment/threatening")] double harassmentThreatening,
            [JsonProperty("self-harm")] double selfHarm,
            [JsonProperty("self-harm/intent")] double selfHarmIntent,
            [JsonProperty("self-harm/instructions")] double selfHarmInstructions,
            [JsonProperty("sexual")] double sexual,
            [JsonProperty("sexual/minors")] double sexualMinors,
            [JsonProperty("violence")] double violence,
            [JsonProperty("violence/graphic")] double violenceGraphic)
        {
            Hate = hate;
            HateThreatening = hateThreatening;
            Harassment = harassment;
            HarassmentThreatening = harassmentThreatening;
            SelfHarm = selfHarm;
            SelfHarmIntent = selfHarmIntent;
            SelfHarmInstructions = selfHarmInstructions;
            Sexual = sexual;
            SexualMinors = sexualMinors;
            Violence = violence;
            ViolenceGraphic = violenceGraphic;
        }

        [JsonProperty("hate")]
        public double Hate { get; }

        [JsonProperty("hate/threatening")]
        public double HateThreatening { get; }

        [JsonProperty("harassment")]
        public double Harassment { get; }

        [JsonProperty("harassment/threatening")]
        public double HarassmentThreatening { get; }

        [JsonProperty("self-harm")]
        public double SelfHarm { get; }

        [JsonProperty("self-harm/intent")]
        public double SelfHarmIntent { get; }

        [JsonProperty("self-harm/instructions")]
        public double SelfHarmInstructions { get; }

        [JsonProperty("sexual")]
        public double Sexual { get; }

        [JsonProperty("sexual/minors")]
        public double SexualMinors { get; }

        [JsonProperty("violence")]
        public double Violence { get; }

        [JsonProperty("violence/graphic")]
        public double ViolenceGraphic { get; }

        public override string ToString() =>
            $"Hate: {Hate:0.00 e+00}\n" +
            $"Hate/Threatening: {HateThreatening:0.00 e+00}\n" +
            $"Harassment: {Harassment:0.00 e+00}\n" +
            $"Harassment/Threatening: {HarassmentThreatening:0.00 e+00}\n" +
            $"Self-Harm: {SelfHarm:0.00 e+00}\n" +
            $"Self-Harm/Intent: {SelfHarmIntent:0.00 e+00}\n" +
            $"Self-Harm/Instructions: {SelfHarmInstructions:0.00 e+00}\n" +
            $"Sexual: {Sexual:0.00 e+00}\n" +
            $"Sexual/Minors: {SexualMinors:0.00 e+00}\n" +
            $"Violence: {Violence:0.00 e+00}\n" +
            $"Violence/Graphic: {ViolenceGraphic:0.00 e+00}\n";

        public static implicit operator string(Scores scores) => scores.ToString();
    }
}
