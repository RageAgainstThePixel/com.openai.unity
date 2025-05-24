// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class CreateResponseRequest
    {
        /// <summary>
        /// Text, image, or file inputs to the model, used to generate a response.
        /// </summary>
        [Preserve]
        [JsonProperty("input")]
        public object Input { get; }

        /// <summary>
        /// Model ID used to generate the response, like gpt-4o or o3.
        /// OpenAI offers a wide range of models with different capabilities, performance characteristics, and price points.
        /// Refer to the model guide to browse and compare available models.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// Whether to run the model response in the background. 
        /// </summary>
        [Preserve]
        [JsonProperty("background", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Background { get; }

        /// <summary>
        /// Specify additional output data to include in the model response.<br/>
        /// Currently supported values are:<br/>
        /// - file_search_call.results: Include the search results of the file search tool call.<br/>
        /// - message.input_image.image_url: Include image URLs from the computer call output.<br/>
        /// - computer_call_output.output.image_url: Include image urls from the computer call output.<br/>
        /// </summary>
        [Preserve]
        [JsonProperty("include", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<string> Include { get; }

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
        /// Whether to allow the model to run tool calls in parallel.
        /// </summary>
        [Preserve]
        [JsonProperty("parallel_tool_calls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ParallelToolCalls { get; }

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
        /// Whether to store the generated model response for later retrieval via API.
        /// </summary>
        [Preserve]
        [JsonProperty("store", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Store { get; }

        /// <summary>
        /// If set to true, the model response data will be streamed to the client as it is generated using server-sent events.
        /// </summary>
        [Preserve]
        [JsonProperty("stream", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Stream { get; }

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
        public ChatResponseFormat TextResponseFormat => TextResponseFormatConfiguration ?? ChatResponseFormat.Auto;

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
