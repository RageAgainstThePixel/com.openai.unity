// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Moderations
{
    public sealed class Categories
    {
        [JsonProperty("hate")]
        public bool Hate { get; set; }

        [JsonProperty("hate/threatening")]
        public bool HateThreatening { get; set; }

        [JsonProperty("self-harm")]
        public bool SelfHarm { get; set; }

        [JsonProperty("sexual")]
        public bool Sexual { get; set; }

        [JsonProperty("sexual/minors")]
        public bool SexualMinors { get; set; }

        [JsonProperty("violence")]
        public bool Violence { get; set; }

        [JsonProperty("violence/graphic")]
        public bool ViolenceGraphic { get; set; }
    }
}
