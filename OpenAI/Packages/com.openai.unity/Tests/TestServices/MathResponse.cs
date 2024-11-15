// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI.Tests.StructuredOutput
{
    internal sealed class MathResponse
    {
        [JsonProperty("steps")]
        public IReadOnlyList<MathStep> Steps { get; private set; }

        [JsonProperty("final_answer")]
        public string FinalAnswer { get; private set; }
    }

    internal sealed class MathStep
    {
        [JsonProperty("explanation")]
        public string Explanation { get; private set; }

        [JsonProperty("output")]
        public string Output { get; private set; }
    }
}
