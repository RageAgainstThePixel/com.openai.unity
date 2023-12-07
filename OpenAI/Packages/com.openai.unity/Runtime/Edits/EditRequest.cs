// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Edits
{
    [Preserve]
    [Obsolete("Deprecated")]
    public sealed class EditRequest
    {
        /// <summary>
        /// Creates a new edit request for the provided input, instruction, and parameters.
        /// </summary>
        /// <param name="input">The input text to use as a starting point for the edit.</param>
        /// <param name="instruction">The instruction that tells the model how to edit the prompt.</param>
        /// <param name="editCount">How many edits to generate for the input and instruction.</param>
        /// <param name="temperature">
        /// What sampling temperature to use. Higher values means the model will take more risks.
        /// Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// We generally recommend altering this or top_p but not both.
        /// </param>
        /// <param name="topP">
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the
        /// results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.
        /// </param>
        /// <param name="model">ID of the model to use. Defaults to text-davinci-edit-001.</param>
        [Preserve]
        public EditRequest(
            string input,
            string instruction,
            int? editCount = null,
            double? temperature = null,
            double? topP = null,
            string model = null)
        {
            Model = string.IsNullOrWhiteSpace(model) ? Models.Model.DavinciEdit : model;
            Input = input;
            Instruction = instruction;
            EditCount = editCount;
            Temperature = temperature;
            TopP = topP;
        }

        /// <summary>
        /// ID of the model to use. Defaults to text-davinci-edit-001.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The input text to use as a starting point for the edit.
        /// </summary>
        [Preserve]
        [JsonProperty("input")]
        public string Input { get; }

        /// <summary>
        /// The instruction that tells the model how to edit the prompt.
        /// </summary>
        [Preserve]
        [JsonProperty("instruction")]
        public string Instruction { get; }

        /// <summary>
        /// How many edits to generate for the input and instruction.
        /// </summary>
        [Preserve]
        [JsonProperty("n")]
        public int? EditCount { get; }

        /// <summary>
        /// What sampling temperature to use. Higher values means the model will take more risks.
        /// Try 0.9 for more creative applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// We generally recommend altering this or top_p but not both.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature")]
        public double? Temperature { get; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the
        /// results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.
        /// </summary>
        [Preserve]
        [JsonProperty("top_p")]
        public double? TopP { get; }
    }
}
