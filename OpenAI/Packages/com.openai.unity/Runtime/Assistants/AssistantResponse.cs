// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Assistants
{
    /// <summary>
    /// Purpose-built AI that uses OpenAI's models and calls tools.
    /// </summary>
    [Preserve]
    public sealed class AssistantResponse : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal AssistantResponse(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixTimeSeconds,
            [JsonProperty("name")] string name,
            [JsonProperty("description")] string description,
            [JsonProperty("model")] string model,
            [JsonProperty("instructions")] string instructions,
            [JsonProperty("tools")] IReadOnlyList<Tool> tools,
            [JsonProperty("tool_resources")] ToolResources toolResources,
            [JsonProperty("metadata")] Dictionary<string, string> metadata,
            [JsonProperty("temperature")] double temperature,
            [JsonProperty("top_p")] double topP,
            [JsonProperty("response_format")][JsonConverter(typeof(ResponseFormatConverter))] ChatResponseFormat responseFormat)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixTimeSeconds = createdAtUnixTimeSeconds;
            Name = name;
            Description = description;
            Model = model;
            Instructions = instructions;
            Tools = tools;
            ToolResources = toolResources;
            Metadata = metadata;
            Temperature = temperature;
            TopP = topP;
            ResponseFormat = responseFormat;
        }

        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type, which is always assistant.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the assistant was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixTimeSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The name of the assistant.
        /// The maximum length is 256 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// The description of the assistant.
        /// The maximum length is 512 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("description")]
        public string Description { get; }

        /// <summary>
        /// ID of the model to use.
        /// You can use the List models API to see all of your available models,
        /// or see our Model overview for descriptions of them.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The system instructions that the assistant uses.
        /// The maximum length is 32768 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("instructions")]
        public string Instructions { get; }

        /// <summary>
        /// A list of tool enabled on the assistant.
        /// There can be a maximum of 128 tools per assistant.
        /// Tools can be of types 'code_interpreter', 'retrieval', or 'function'.
        /// </summary>
        [Preserve]
        [JsonProperty("tools")]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// A set of resources that are used by the assistant's tools.
        /// The resources are specific to the type of tool.
        /// For example, the code_interpreter tool requires a list of file IDs,
        /// while the file_search tool requires a list of vector store IDs.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_resources")]
        public ToolResources ToolResources { get; }

        /// <summary>
        /// A list of file IDs attached to this assistant.
        /// There can be a maximum of 20 files attached to the assistant.
        /// Files are ordered by their creation date in ascending order.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Files removed from Assistants. Files now belong to ToolResources.")]
        public IReadOnlyList<string> FileIds => null;

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature")]
        public double Temperature { get; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// </summary>
        [Preserve]
        [JsonProperty("top_p")]
        public double TopP { get; }

        /// <summary>
        /// Specifies the format that the model must output.
        /// Setting to <see cref="ChatResponseFormat.Json"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.
        /// </summary>
        /// <remarks>
        /// Important: When using JSON mode you must still instruct the model to produce JSON yourself via some conversation message,
        /// for example via your system message. If you don't do this, the model may generate an unending stream of
        /// whitespace until the generation reaches the token limit, which may take a lot of time and give the appearance
        /// of a "stuck" request. Also note that the message content may be partial (i.e. cut off) if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </remarks>
        [Preserve]
        [JsonProperty("response_format")]
        [JsonConverter(typeof(ResponseFormatConverter))]
        public ChatResponseFormat ResponseFormat { get; }

        [Preserve]
        public static implicit operator string(AssistantResponse assistant) => assistant?.Id;

        [Preserve]
        public override string ToString() => Id;
    }
}
