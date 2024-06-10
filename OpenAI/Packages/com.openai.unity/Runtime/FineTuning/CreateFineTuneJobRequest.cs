// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using UnityEngine.Scripting;

namespace OpenAI.FineTuning
{
    [Preserve]
    public sealed class CreateFineTuneJobRequest
    {
        [Preserve]
        public CreateFineTuneJobRequest(
            Model model,
            string trainingFileId,
            HyperParameters hyperParameters = null,
            string suffix = null,
            string validationFileId = null)
        {
            Model = model ?? Models.Model.GPT4_Turbo;
            TrainingFileId = trainingFileId;
            HyperParameters = hyperParameters;
            Suffix = suffix;
            ValidationFileId = validationFileId;
        }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; set; }

        [Preserve]
        [JsonProperty("training_file")]
        public string TrainingFileId { get; set; }

        [Preserve]
        [JsonProperty("hyperparameters")]
        public HyperParameters HyperParameters { get; set; }

        [Preserve]
        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [Preserve]
        [JsonProperty("validation_file")]
        public string ValidationFileId { get; set; }
    }
}
