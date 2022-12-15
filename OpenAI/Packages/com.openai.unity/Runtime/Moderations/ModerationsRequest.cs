// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;

namespace OpenAI.Moderations
{
    public sealed class ModerationsRequest
    {
        public ModerationsRequest(string input, Model model = null)
        {
            Input = input;
            Model = model;
        }

        [JsonProperty("input")]
        public string Input { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }
    }
}
