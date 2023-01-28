// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Categories
    {
        [JsonConstructor]
        public Categories(bool hate, bool hateThreatening, bool selfHarm, bool sexual, bool sexualMinors, bool violence, bool violenceGraphic)
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
        public bool Hate { get; }

        [JsonProperty("hate/threatening")]
        public bool HateThreatening { get; }

        [JsonProperty("self-harm")]
        public bool SelfHarm { get; }

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
