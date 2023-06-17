using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Chat
{
    /// <summary>
    /// <see href="https://platform.openai.com/docs/guides/gpt/function-calling"/>
    /// </summary>
    public class Function
    {
        /// <summary>
        /// Creates a new function description to insert into a chat conversation.
        /// </summary>
        /// <param name="name">
        /// Required. The name of the function to generate arguments for based on the context in a message.<br/>
        /// May contain a-z, A-Z, 0-9, underscores and dashes, with a maximum length of 64 characters. Recommended to not begin with a number or a dash.
        /// </param>
        /// <param name="description">
        /// An optional description of the function, used by the API to determine if it is useful to include in the response.
        /// </param>
        /// <param name="parameters">
        /// An optional JSON object describing the parameters of the function that the model should generate in JSON schema format (json-schema.org).
        /// </param>
        /// <param name="arguments">
        /// The arguments to use when calling the function.
        /// </param>
        [JsonConstructor]
        public Function(
            [JsonProperty("name")] string name,
            [JsonProperty("description")] string description = null,
            [JsonProperty("parameters")] JToken parameters = null,
            [JsonProperty("arguments")] JToken arguments = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} cannot be null or whitespace.", nameof(name));
            }

            if (name.Length > 64)
            {
                throw new ArgumentException($"{nameof(name)} cannot be longer than 64 characters.", nameof(name));
            }

            Name = name;
            Description = description;
            Parameters = parameters;
            Arguments = arguments;
        }


        /// <summary>
        /// The name of the function to generate arguments for.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores and dashes, with a maximum length of 64 characters.
        /// Recommended to not begin with a number or a dash.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The optional description of the function.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// The optional parameters of the function.
        /// Describe the parameters that the model should generate in JSON schema format (json-schema.org).
        /// </summary>
        [JsonProperty("parameters")]
        public JToken Parameters { get; private set; }

        /// <summary>
        /// The arguments to use when calling the function.
        /// </summary>
        [JsonProperty("arguments")]
        public JToken Arguments { get; private set; }
    }
}
