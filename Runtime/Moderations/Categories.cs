// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Moderations
{
    [Preserve]
    public sealed class Categories
    {
        [Preserve]
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

        [Preserve]
        [JsonProperty("hate")]
        public bool Hate { get; }

        [Preserve]
        [JsonProperty("hate/threatening")]
        public bool HateThreatening { get; }

        [Preserve]
        [JsonProperty("harassment")]
        public bool Harassment { get; }

        [Preserve]
        [JsonProperty("harassment/threatening")]
        public bool HarassmentThreatening { get; }

        [Preserve]
        [JsonProperty("self-harm")]
        public bool SelfHarm { get; }

        [Preserve]
        [JsonProperty("self-harm/intent")]
        public bool SelfHarmIntent { get; }

        [Preserve]
        [JsonProperty("self-harm/instructions")]
        public bool SelfHarmInstructions { get; }

        [Preserve]
        [JsonProperty("sexual")]
        public bool Sexual { get; }

        [Preserve]
        [JsonProperty("sexual/minors")]
        public bool SexualMinors { get; }

        [Preserve]
        [JsonProperty("violence")]
        public bool Violence { get; }

        [Preserve]
        [JsonProperty("violence/graphic")]
        public bool ViolenceGraphic { get; }
    }
}
