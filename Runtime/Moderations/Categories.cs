// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Categories
    {
        [JsonConstructor]
        public Categories(
            [JsonProperty("hate")] bool hate,
            [JsonProperty("hate/threatening")] bool hateThreatening,
            [JsonProperty("harassment")] bool harassment,
            [JsonProperty("harassment/threatening")] bool harassmentThreatening,
            [JsonProperty("self-harm")] bool selfHarm,
            [JsonProperty("self-harm/intent")] bool selfHarmIntent,
            [JsonProperty("self-harm/instructions")] bool selfHarmInstructions,
            [JsonProperty("sexual")] bool sexual,
            [JsonProperty("sexual/minors")] bool sexualMinors,
            [JsonProperty("violence")] bool violence,
            [JsonProperty("violence/graphic")] bool violenceGraphic)
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
        public bool Hate { get; }

        [JsonProperty("hate/threatening")]
        public bool HateThreatening { get; }

        [JsonProperty("harassment")]
        public bool Harassment { get; }

        [JsonProperty("harassment/threatening")]
        public bool HarassmentThreatening { get; }

        [JsonProperty("self-harm")]
        public bool SelfHarm { get; }

        [JsonProperty("self-harm/intent")]
        public bool SelfHarmIntent { get; }

        [JsonProperty("self-harm/instructions")]
        public bool SelfHarmInstructions { get; }

        [JsonProperty("sexual")]
        public bool Sexual { get; }

        [JsonProperty("sexual/minors")]
        public bool SexualMinors { get; }

        [JsonProperty("violence")]
        public bool Violence { get; }

        [JsonProperty("violence/graphic")]
        public bool ViolenceGraphic { get; }
    }
}
