// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenAI
{
    /// <summary>
    /// Represents a response from the Classification API.
    /// <see href="https://beta.openai.com/docs/api-reference/classifications">the OpenAI docs</see>.
    /// </summary>
    public sealed class ClassificationResponse : BaseResponse
    {
        [JsonProperty("completion")]
        public string Completion { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("selected_examples")]
        public IReadOnlyList<SelectedExample> SelectedExamples { get; set; }
    }
}
