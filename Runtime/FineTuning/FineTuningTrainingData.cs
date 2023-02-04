// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;

namespace OpenAI.FineTuning
{
    /// <summary>
    /// To fine-tune a model, you'll need a set of training examples that each consist of a single input
    /// ("prompt") and its associated output ("completion"). This is notably different from using our base models,
    /// where you might input detailed instructions or multiple examples in a single prompt.
    /// <see href="https://beta.openai.com/docs/guides/fine-tuning/specific-guidelines"/>
    /// </summary>
    [Serializable]
    public class FineTuningTrainingData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="prompt">Prompt text.</param>
        /// <param name="completion">
        /// The ideal completion text which is prefixed with a space, with <see cref="completionSuffix"/>.
        /// </param>
        /// <param name="promptSuffix">
        /// Each prompt should end with a fixed separator to inform the model when the prompt ends and the completion begins.
        /// A simple separator which generally works well is "\n\n###\n\n". The separator should not appear elsewhere in any prompt.
        /// The default is "\n\n###\n\n".
        /// </param>
        /// <param name="completionSuffix">
        /// Optional, Each completion should end with a fixed stop sequence to inform the model when the completion ends.
        /// A stop sequence could be "\n", "###", or any other token that does not appear in any completion. Default is " END".
        /// </param>
        public FineTuningTrainingData(string prompt, string completion, string promptSuffix = "\\n\\n###\\n\\n", string completionSuffix = " END")
        {
            this.prompt = prompt;
            this.promptSuffix = promptSuffix;
            this.completion = completion;
            this.completionSuffix = completionSuffix;
        }

        [SerializeField]
        [TextArea(1, 20)]
        private string prompt;

        [SerializeField]
        private string promptSuffix;

        /// <summary>
        /// Prompt text.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt => $"{prompt}{promptSuffix.Replace("\\n", "\n")}";

        [SerializeField]
        [TextArea(1, 20)]
        private string completion;

        [SerializeField]
        private string completionSuffix;

        /// <summary>
        /// The ideal completion text.
        /// </summary>
        [JsonProperty("completion")]
        public string Completion => $" {completion}{completionSuffix.Replace("\\n", "\n")}";

        public static implicit operator string(FineTuningTrainingData data) => data.ToString();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
