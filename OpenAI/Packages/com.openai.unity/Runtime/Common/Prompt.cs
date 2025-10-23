// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Prompt
    {
        [Preserve]
        [JsonConstructor]
        public Prompt(
            [JsonProperty("id")] string id,
            [JsonProperty("variables")] Dictionary<string, object> variables = null,
            [JsonProperty("version")] string version = null)
        {
            Id = id;
            Variables = variables;
            Version = version;
        }

        /// <summary>
        /// The unique identifier of the prompt template to use.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// Optional map of values to substitute in for variables in your prompt.
        /// The substitution values can either be strings, or other Response input types like images or files.
        /// </summary>
        [Preserve]
        [JsonProperty("variables")]
        public IReadOnlyDictionary<string, object> Variables { get; }

        /// <summary>
        /// Optional version of the prompt template.
        /// </summary>
        [Preserve]
        [JsonProperty("version")]
        public string Version { get; }
    }
}
