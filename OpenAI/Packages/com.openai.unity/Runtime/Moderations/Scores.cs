// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Moderations
{
    [Preserve]
    public sealed class Scores
    {
        [Preserve]
        [JsonConstructor]
        internal Scores(
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

        [Preserve]
        [JsonProperty("hate")]
        public double Hate { get; }

        [Preserve]
        [JsonProperty("hate/threatening")]
        public double HateThreatening { get; }

        [Preserve]
        [JsonProperty("harassment")]
        public double Harassment { get; }

        [Preserve]
        [JsonProperty("harassment/threatening")]
        public double HarassmentThreatening { get; }

        [Preserve]
        [JsonProperty("self-harm")]
        public double SelfHarm { get; }

        [Preserve]
        [JsonProperty("self-harm/intent")]
        public double SelfHarmIntent { get; }

        [Preserve]
        [JsonProperty("self-harm/instructions")]
        public double SelfHarmInstructions { get; }

        [Preserve]
        [JsonProperty("sexual")]
        public double Sexual { get; }

        [Preserve]
        [JsonProperty("sexual/minors")]
        public double SexualMinors { get; }

        [Preserve]
        [JsonProperty("violence")]
        public double Violence { get; }

        [Preserve]
        [JsonProperty("violence/graphic")]
        public double ViolenceGraphic { get; }

        [Preserve]
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

        [Preserve]
        public static implicit operator string(Scores scores) => scores.ToString();
    }
}
