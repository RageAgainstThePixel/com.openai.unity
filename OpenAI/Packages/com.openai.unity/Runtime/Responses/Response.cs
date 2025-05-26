// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class Response : BaseResponse
    {
        [Preserve]
        [JsonConstructor]
        internal Response(
            [JsonProperty("id")] string id,
            [JsonProperty("object")] string @object,
            [JsonProperty("created_at")] int createdAtUnixSeconds,
            [JsonProperty("background")] bool? background,
            [JsonProperty("error")] Error error,
            [JsonProperty("incomplete_details")] IncompleteDetails incompleteDetails,
            [JsonProperty("output")] IReadOnlyList<IResponseItem> output,
            [JsonProperty("output_text")] string outputText,
            [JsonProperty("usage")] ResponseUsage usage,
            [JsonProperty("parallel_tool_calls")] bool? parallelToolCalls,
            [JsonProperty("instructions")] string instructions,
            [JsonProperty("max_output_tokens")] int? maxOutputTokens,
            [JsonProperty("metadata")] IReadOnlyDictionary<string, string> metadata,
            [JsonProperty("model")] string model,
            [JsonProperty("previous_response_id")] string previousResponseId,
            [JsonProperty("reasoning")] Reasoning reasoning,
            [JsonProperty("service_tier")] string serviceTier,
            [JsonProperty("status")] ResponseStatus status,
            [JsonProperty("temperature")] double? temperature,
            [JsonProperty("text")] TextResponseFormatConfiguration textResponseFormatConfiguration,
            [JsonProperty("tool_choice")] object toolChoice,
            [JsonProperty("tools")] IReadOnlyList<Tool> tools,
            [JsonProperty("top_p")] double? topP,
            [JsonProperty("truncation")] Truncation truncation,
            [JsonProperty("user")] string user)
        {
            Id = id;
            Object = @object;
            CreatedAtUnixSeconds = createdAtUnixSeconds;
            Background = background;
            Error = error;
            IncompleteDetails = incompleteDetails;
            Output = output;
            OutputText = outputText;
            Usage = usage;
            ParallelToolCalls = parallelToolCalls;
            Instructions = instructions;
            MaxOutputTokens = maxOutputTokens;
            Metadata = metadata;
            Model = model;
            PreviousResponseId = previousResponseId;
            Reasoning = reasoning;
            ServiceTier = serviceTier;
            Status = status;
            Temperature = temperature;
            TextResponseFormatConfiguration = textResponseFormatConfiguration;
            ToolChoice = toolChoice;
            Tools = tools;
            TopP = topP;
            Truncation = truncation;
            User = user;
        }

        /// <summary>
        /// Unique identifier for this Response.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The object type of this resource - always set to 'response'.
        /// </summary>
        [Preserve]
        [JsonProperty("object")]
        public string Object { get; }

        /// <summary>
        /// Unix timestamp (in seconds) of when this Response was created.
        /// </summary>
        [Preserve]
        [JsonProperty("created_at")]
        public int CreatedAtUnixSeconds { get; }

        [Preserve]
        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixSeconds).UtcDateTime;

        /// <summary>
        /// Whether to run the model response in the background.
        /// </summary>
        [Preserve]
        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Background { get; }

        /// <summary>
        /// An error object returned when the model fails to generate a Response.
        /// </summary>
        [Preserve]
        [JsonProperty("error", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Error Error { get; }

        [Preserve]
        [JsonProperty("incomplete_details", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IncompleteDetails IncompleteDetails { get; }

        [Preserve]
        [JsonProperty("output", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IResponseItem> Output { get; }

        [Preserve]
        [JsonProperty("output_text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OutputText { get; }

        [Preserve]
        [JsonProperty("usage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseUsage Usage { get; }

        /// <summary>
        /// Whether to allow the model to run tool calls in parallel.
        /// </summary>
        [Preserve]
        [JsonProperty("parallel_tool_calls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ParallelToolCalls { get; }

        /// <summary>
        /// Inserts a system (or developer) message as the first item in the model's context.
        /// When using along with `previous_response_id`,
        /// the instructions from a previous response will not be carried over to the next response.
        /// This makes it simple to swap out system (or developer) messages in new responses.
        /// </summary>
        [Preserve]
        [JsonProperty("instructions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Instructions { get; }

        /// <summary>
        /// An upper bound for the number of tokens that can be generated for a
        /// response, including visible output tokens and reasoning
        /// tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("max_output_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MaxOutputTokens { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format,
        /// and querying for objects via API or the dashboard.
        /// Keys are strings with a maximum length of 64 characters.
        /// Values are strings with a maximum length of 512 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// Model ID used to generate the response, like gpt-4o or o3.
        /// OpenAI offers a wide range of models with different capabilities, performance characteristics, and price points.
        /// Refer to the model guide to browse and compare available models.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// The unique ID of the previous response to the model.
        /// Use this to create multi-turn conversations.
        /// </summary>
        [JsonProperty("previous_response_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string PreviousResponseId { get; }

        /// <summary>
        /// Configuration options for reasoning models.
        /// </summary>
        /// <remarks>
        /// o-series models only!
        /// </remarks>
        [Preserve]
        [JsonProperty("reasoning", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Reasoning Reasoning { get; }

        /// <summary>
        /// Specifies the latency tier to use for processing the request. This parameter is relevant for customers subscribed to the scale tier service:<br/>
        /// - If set to 'auto', and the Project is Scale tier enabled, the system will utilize scale tier credits until they are exhausted.<br/>
        /// - If set to 'auto', and the Project is not Scale tier enabled, the request will be processed using the default service tier with a lower uptime SLA and no latency guarantee.<br/>
        /// - If set to 'default', the request will be processed using the default service tier with a lower uptime SLA and no latency guarantee.<br/>
        /// - When not set, the default behavior is 'auto'.<br/>
        /// When this parameter is set, the response body will include the service_tier utilized.
        /// </summary>
        [Preserve]
        [JsonProperty("service_tier", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ServiceTier { get; set; }

        /// <summary>
        /// The status of the response generation.
        /// </summary>
        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will
        /// make it more focused and deterministic.
        /// We generally recommend altering this or top_p but not both.<br/>
        /// Defaults to 1
        /// </summary>
        [Preserve]
        [JsonProperty("temperature", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? Temperature { get; }

        /// <summary>
        /// Configuration options for a text response from the model. Can be plain text or structured JSON data.
        /// </summary>
        [Preserve]
        [JsonConverter(typeof(TextResponseFormatConverter))]
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TextResponseFormatConfiguration TextResponseFormatConfiguration { get; internal set; }

        [Preserve]
        [JsonIgnore]
        public TextResponseFormat TextResponseFormat => TextResponseFormatConfiguration ?? TextResponseFormat.Auto;

        /// <summary>
        /// How the model should select which tool (or tools) to use when generating a response.
        /// See the tools parameter to see how to specify which tools the model can call.
        /// </summary>
        [Preserve]
        [JsonProperty("tool_choice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object ToolChoice { get; }

        /// <summary>
        /// A list of tools the model may call. Currently, only functions are supported as a tool.
        /// Use this to provide a list of functions the model may generate JSON inputs for.
        /// </summary>
        [Preserve]
        [JsonProperty("tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.<br/>
        /// Defaults to 1
        /// </summary>
        [Preserve]
        [JsonProperty("top_p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? TopP { get; }

        /// <summary>
        /// The truncation strategy to use for the model response.<br/>
        /// - Auto: If the context of this response and previous ones exceeds the model's context window size,
        /// the model will truncate the response to fit the context window by dropping input items in the middle of the conversation.<br/>
        /// - Disabled (default): If a model response will exceed the context window size for a model, the request will fail with a 400 error.
        /// </summary>
        [Preserve]
        [JsonProperty("truncation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Truncation Truncation { get; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        [Preserve]
        [JsonProperty("user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string User { get; }
    }
}
