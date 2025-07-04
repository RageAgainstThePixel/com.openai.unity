// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Assistants
{
    [Preserve]
    public sealed class CreateAssistantRequest
    {
        [Obsolete("use new .ctr")]
        public CreateAssistantRequest(
            AssistantResponse assistant,
            string model,
            string name,
            string description,
            string instructions,
            IEnumerable<Tool> tools,
            ToolResources toolResources,
            IReadOnlyDictionary<string, string> metadata,
            double? temperature,
            double? topP,
            JsonSchema jsonSchema,
            TextResponseFormat? responseFormat = null)
            : this(
                string.IsNullOrWhiteSpace(model) ? assistant.Model : model,
                string.IsNullOrWhiteSpace(name) ? assistant.Name : name,
                string.IsNullOrWhiteSpace(description) ? assistant.Description : description,
                string.IsNullOrWhiteSpace(instructions) ? assistant.Instructions : instructions,
                tools ?? assistant.Tools,
                toolResources ?? assistant.ToolResources,
                metadata ?? assistant.Metadata,
                temperature ?? (assistant.ReasoningEffort > 0 ? null : assistant.Temperature),
                topP ?? assistant.TopP,
                0,
                jsonSchema ?? assistant.ResponseFormatObject?.JsonSchema,
                responseFormat ?? assistant.ResponseFormat)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assistant"></param>
        /// <param name="model">
        /// ID of the model to use.
        /// You can use the List models API to see all of your available models,
        /// or see our Model overview for descriptions of them.
        /// </param>
        /// <param name="name">
        /// The name of the assistant.
        /// The maximum length is 256 characters.
        /// </param>
        /// <param name="description">
        /// The description of the assistant.
        /// The maximum length is 512 characters.
        /// </param>
        /// <param name="instructions">
        /// The system instructions that the assistant uses.
        /// The maximum length is 32768 characters.
        /// </param>
        /// <param name="tools">
        /// A list of tool enabled on the assistant.
        /// There can be a maximum of 128 tools per assistant.
        /// Tools can be of types 'code_interpreter', 'retrieval', or 'function'.
        /// </param>
        /// <param name="toolResources">
        /// A set of resources that are used by Assistants and Threads. The resources are specific to the type of tool.
        /// For example, the <see cref="Tool.CodeInterpreter"/> requres a list of file ids,
        /// While the <see cref="Tool.FileSearch"/> requires a list vector store ids.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="temperature">
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </param>
        /// <param name="topP">
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.
        /// </param>
        /// <param name="reasoningEffort">
        /// Constrains effort on reasoning for reasoning models.
        /// Currently supported values are low, medium, and high.
        /// Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.
        /// </param>
        /// <param name="jsonSchema">
        /// The <see cref="JsonSchema"/> to use for structured JSON outputs.<br/>
        /// <see href="https://platform.openai.com/docs/guides/structured-outputs"/><br/>
        /// <see href="https://json-schema.org/overview/what-is-jsonschema"/>
        /// </param>
        /// <param name="responseFormat">
        /// Specifies the format that the model must output.
        /// Setting to <see cref="TextResponseFormat.Json"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.<br/>
        /// Important: When using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.
        /// Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit,
        /// resulting in a long-running and seemingly "stuck" request. Also note that the message content may be partially cut off if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </param>
        [Preserve]
        public CreateAssistantRequest(
            AssistantResponse assistant,
            string model = null,
            string name = null,
            string description = null,
            string instructions = null,
            IEnumerable<Tool> tools = null,
            ToolResources toolResources = null,
            IReadOnlyDictionary<string, string> metadata = null,
            double? temperature = null,
            double? topP = null,
            ReasoningEffort reasoningEffort = 0,
            JsonSchema jsonSchema = null,
            TextResponseFormat? responseFormat = null)
        : this(
            string.IsNullOrWhiteSpace(model) ? assistant.Model : model,
            string.IsNullOrWhiteSpace(name) ? assistant.Name : name,
            string.IsNullOrWhiteSpace(description) ? assistant.Description : description,
            string.IsNullOrWhiteSpace(instructions) ? assistant.Instructions : instructions,
            tools ?? assistant.Tools,
            toolResources ?? assistant.ToolResources,
            metadata ?? assistant.Metadata,
            temperature ?? (assistant.ReasoningEffort > 0 ? null : assistant.Temperature),
            topP ?? (assistant.ReasoningEffort > 0 ? null : assistant.TopP),
            reasoningEffort,
            jsonSchema ?? assistant.ResponseFormatObject?.JsonSchema,
            responseFormat ?? assistant.ResponseFormat)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">
        /// ID of the model to use.
        /// You can use the List models API to see all of your available models,
        /// or see our Model overview for descriptions of them.
        /// </param>
        /// <param name="name">
        /// The name of the assistant.
        /// The maximum length is 256 characters.
        /// </param>
        /// <param name="description">
        /// The description of the assistant.
        /// The maximum length is 512 characters.
        /// </param>
        /// <param name="instructions">
        /// The system instructions that the assistant uses.
        /// The maximum length is 256,000 characters.
        /// </param>
        /// <param name="tools">
        /// A list of tool enabled on the assistant.
        /// There can be a maximum of 128 tools per assistant.
        /// Tools can be of types 'code_interpreter', 'retrieval', or 'function'.
        /// </param>
        /// <param name="toolResources">
        /// A set of resources that are used by Assistants and Threads. The resources are specific to the type of tool.
        /// For example, the <see cref="Tool.CodeInterpreter"/> requres a list of file ids,
        /// While the <see cref="Tool.FileSearch"/> requires a list vector store ids.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="temperature">
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </param>
        /// <param name="topP">
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.
        /// </param>
        /// <param name="reasoningEffort">
        /// Constrains effort on reasoning for reasoning models.
        /// Currently supported values are low, medium, and high.
        /// Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.
        /// </param>
        /// <param name="jsonSchema">
        /// The <see cref="JsonSchema"/> to use for structured JSON outputs.<br/>
        /// <see href="https://platform.openai.com/docs/guides/structured-outputs"/><br/>
        /// <see href="https://json-schema.org/overview/what-is-jsonschema"/>
        /// </param>
        /// <param name="responseFormat">
        /// Specifies the format that the model must output.
        /// Setting to <see cref="TextResponseFormat.Json"/> or <see cref="TextResponseFormat.JsonSchema"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.<br/>
        /// Important: When using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.
        /// Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit,
        /// resulting in a long-running and seemingly "stuck" request. Also note that the message content may be partially cut off if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </param>
        [Preserve]
        public CreateAssistantRequest(
            string model = null,
            string name = null,
            string description = null,
            string instructions = null,
            IEnumerable<Tool> tools = null,
            ToolResources toolResources = null,
            IReadOnlyDictionary<string, string> metadata = null,
            double? temperature = null,
            double? topP = null,
            ReasoningEffort reasoningEffort = 0,
            JsonSchema jsonSchema = null,
            TextResponseFormat responseFormat = TextResponseFormat.Text)
        {
            Model = string.IsNullOrWhiteSpace(model) ? Models.Model.GPT4o : model;
            Name = name;
            Description = description;
            Instructions = instructions;
            tools.ProcessTools<Tool>(null, out var toolList, out _);
            Tools = toolList;
            ToolResources = toolResources;
            Metadata = metadata;
            Temperature = reasoningEffort > 0 ? null : temperature;
            TopP = reasoningEffort > 0 ? null : topP;
            ReasoningEffort = reasoningEffort;

            if (jsonSchema != null)
            {
                ResponseFormatObject = jsonSchema;
            }
            else
            {
                ResponseFormatObject = responseFormat;
            }
        }

        /// <summary>
        /// ID of the model to use.
        /// You can use the List models API to see all of your available models,
        /// or see our Model overview for descriptions of them.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

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
        /// The system instructions that the assistant uses.
        /// The maximum length is 256,000 characters.
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
        [JsonProperty("tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// A set of resources that are used by Assistants and Threads. The resources are specific to the type of tool.
        /// For example, the <see cref="Tool.CodeInterpreter"/> requres a list of file ids,
        /// While the <see cref="Tool.FileSearch"/> requires a list vector store ids.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_resources", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ToolResources ToolResources { get; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? Temperature { get; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// </summary>
        [Preserve]
        [JsonProperty("top_p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? TopP { get; }

        /// <summary>
        /// Constrains effort on reasoning for reasoning models.
        /// Currently supported values are low, medium, and high.
        /// Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.
        /// </summary>
        [Preserve]
        [JsonProperty("reasoning_effort", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReasoningEffort ReasoningEffort { get; }

        /// <summary>
        /// Specifies the format that the model must output.
        /// Setting to <see cref="TextResponseFormat.Json"/> or <see cref="TextResponseFormat.JsonSchema"/> enables JSON mode,
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
        [JsonConverter(typeof(TextResponseFormatConfigurationConverter))]
        [JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TextResponseFormatConfiguration ResponseFormatObject { get; internal set; }

        [Preserve]
        [JsonIgnore]
        public TextResponseFormat ResponseFormat => ResponseFormatObject ?? TextResponseFormat.Auto;

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
