// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class JsonSchema
    {
        /// <inheritdoc />
        public JsonSchema(string name, string schema, string description = null, bool strict = true)
            : this(name, JToken.Parse(schema), description, strict) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">
        /// The name of the response format. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
        /// </param>
        /// <param name="schema">
        /// The schema for the response format, described as a JSON Schema object.
        /// </param>
        /// <param name="description">
        /// A description of what the response format is for, used by the model to determine how to respond in the format.
        /// </param>
        /// <param name="strict">
        /// Whether to enable strict schema adherence when generating the output.
        /// If set to true, the model will always follow the exact schema defined in the schema field.
        /// Only a subset of JSON Schema is supported when strict is true.
        /// </param>
        [Preserve]
        [JsonConstructor]
        public JsonSchema(
            [JsonProperty("name")] string name,
            [JsonProperty("schema")] JToken schema,
            [JsonProperty("description")] string description = null,
            [JsonProperty("strict")] bool strict = true)
        {
            Name = name;
            Description = description;
            Strict = strict;
            Schema = schema;
        }

        /// <summary>
        /// The name of the response format. Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// A description of what the response format is for, used by the model to determine how to respond in the format.
        /// </summary>
        [Preserve]
        [JsonProperty("description")]
        public string Description { get; }

        /// <summary>
        /// Whether to enable strict schema adherence when generating the output.
        /// If set to true, the model will always follow the exact schema defined in the schema field.
        /// </summary>
        /// <remarks>
        /// Only a subset of JSON Schema is supported when strict is true.
        /// </remarks>
        [Preserve]
        [JsonProperty("strict")]
        public bool Strict { get; }

        /// <summary>
        /// The schema for the response format, described as a JSON Schema object.
        /// </summary>
        [Preserve]
        [JsonProperty("schema")]
        public JToken Schema { get; }

        [Preserve]
        public static implicit operator ResponseFormatObject(JsonSchema jsonSchema) => new(jsonSchema);
    }
}
