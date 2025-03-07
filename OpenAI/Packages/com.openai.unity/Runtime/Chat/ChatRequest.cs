// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Chat
{
    [Preserve]
    public sealed class ChatRequest
    {
        /// <inheritdoc />
        [Preserve]
        public ChatRequest(
            IEnumerable<Message> messages,
            IEnumerable<Tool> tools,
            string toolChoice = null,
            string model = null,
            double? frequencyPenalty = null,
            IReadOnlyDictionary<string, double> logitBias = null,
            int? maxTokens = null,
            int? number = null,
            double? presencePenalty = null,
            ChatResponseFormat responseFormat = ChatResponseFormat.Auto,
            int? seed = null,
            string[] stops = null,
            double? temperature = null,
            double? topP = null,
            int? topLogProbs = null,
            bool? parallelToolCalls = null,
            JsonSchema jsonSchema = null,
            AudioConfig audioConfig = null,
            ReasoningEffort? reasoningEffort = null,
            string user = null)
            : this(messages, model, frequencyPenalty, logitBias, maxTokens, number, presencePenalty,
                responseFormat, seed, stops, temperature, topP, topLogProbs, parallelToolCalls, jsonSchema, audioConfig, reasoningEffort, user)
        {
            var toolList = tools?.ToList();

            if (toolList is { Count: > 0 })
            {
                if (string.IsNullOrWhiteSpace(toolChoice))
                {
                    ToolChoice = "auto";
                }
                else
                {
                    if (!toolChoice.Equals("none") &&
                        !toolChoice.Equals("required") &&
                        !toolChoice.Equals("auto"))
                    {
                        var tool = toolList.FirstOrDefault(t => t.Function.Name.Contains(toolChoice)) ??
                            throw new ArgumentException($"The specified tool choice '{toolChoice}' was not found in the list of tools");
                        ToolChoice = new { type = "function", function = new { name = tool.Function.Name } };
                    }
                    else
                    {
                        ToolChoice = toolChoice;
                    }
                }

                foreach (var tool in toolList)
                {
                    if (tool?.Function?.Arguments != null)
                    {
                        // just in case clear any lingering func args.
                        tool.Function.Arguments = null;
                    }
                }
            }

            Tools = toolList?.ToList();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messages">
        /// The list of messages for the current chat session.
        /// </param>
        /// <param name="model">
        /// Id of the model to use.
        /// </param>
        /// <param name="temperature">
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will
        /// make it more focused and deterministic.
        /// We generally recommend altering this or top_p but not both.<br/>
        /// Defaults to 1
        /// </param>
        /// <param name="topP">
        /// An alternative to sampling with temperature, called nucleus sampling,
        /// where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.<br/>
        /// Defaults to 1
        /// </param>
        /// <param name="number">
        /// How many chat completion choices to generate for each input message.<br/>
        /// Defaults to 1
        /// </param>
        /// <param name="seed"></param>
        /// <param name="stops">
        /// Up to 4 sequences where the API will stop generating further tokens.
        /// </param>
        /// <param name="maxTokens">
        /// An upper bound for the number of tokens that can be generated for a completion, including visible output tokens and reasoning tokens.
        /// </param>
        /// <param name="presencePenalty">
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on whether they appear in the text so far,
        /// increasing the model's likelihood to talk about new topics.<br/>
        /// Defaults to 0
        /// </param>
        /// <param name="responseFormat">
        /// An object specifying the format that the model must output.
        /// Setting to <see cref="ChatResponseFormat.Json"/> or <see cref="ChatResponseFormat.JsonSchema"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.
        /// </param>
        /// <param name="frequencyPenalty">
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.<br/>
        /// Defaults to 0
        /// </param>
        /// <param name="logitBias">
        /// Modify the likelihood of specified tokens appearing in the completion.
        /// Accepts a json object that maps tokens(specified by their token ID in the tokenizer)
        /// to an associated bias value from -100 to 100. Mathematically, the bias is added to the logits
        /// generated by the model prior to sampling.The exact effect will vary per model, but values between
        /// -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result
        /// in a ban or exclusive selection of the relevant token.<br/>
        /// Defaults to null
        /// </param>
        /// <param name="topLogProbs">
        /// An integer between 0 and 5 specifying the number of most likely tokens to return at each token position,
        /// each with an associated log probability.
        /// </param>
        /// <param name="jsonSchema">
        /// The <see cref="JsonSchema"/> to use for structured JSON outputs.<br/>
        /// <see href="https://platform.openai.com/docs/guides/structured-outputs"/><br/>
        /// <see href="https://json-schema.org/overview/what-is-jsonschema"/>
        /// </param>
        /// <param name="parallelToolCalls">
        /// Whether to enable parallel function calling during tool use.
        /// </param>
        /// <param name="audioConfig">
        /// Parameters for audio output. <see cref="Chat.AudioConfig"/>.
        /// </param>
        /// <param name="reasoningEffort">
        /// Constrains the effort of reasoning for <see href="https://platform.openai.com/docs/guides/reasoning">Reasoning Models</see>.<br/>
        /// Currently supported values are: Low, Medium, High. Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning response.<br/>
        /// <b>Reasoning models only!</b>
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        [Preserve]
        public ChatRequest(
            IEnumerable<Message> messages,
            string model = null,
            double? frequencyPenalty = null,
            IReadOnlyDictionary<string, double> logitBias = null,
            int? maxTokens = null,
            int? number = null,
            double? presencePenalty = null,
            ChatResponseFormat responseFormat = ChatResponseFormat.Auto,
            int? seed = null,
            string[] stops = null,
            double? temperature = null,
            double? topP = null,
            int? topLogProbs = null,
            bool? parallelToolCalls = null,
            JsonSchema jsonSchema = null,
            AudioConfig audioConfig = null,
            ReasoningEffort? reasoningEffort = null,
            string user = null)
        {
            Messages = messages?.ToList();

            if (Messages?.Count == 0)
            {
                throw new ArgumentNullException(nameof(messages), $"Missing required {nameof(messages)} parameter");
            }

            Model = string.IsNullOrWhiteSpace(model) ? Models.Model.GPT4o : model;

            if (reasoningEffort.HasValue)
            {
                ReasoningEffort = reasoningEffort.Value;
            }

            if (audioConfig != null && !Model.Contains("audio"))
            {
                throw new ArgumentException("Audio settings are only valid for models that support audio output", nameof(audioConfig));
            }

            if (Model.Contains("audio"))
            {
                Modalities = Modality.Text | Modality.Audio;
                AudioConfig = audioConfig ?? new(Voice.Alloy);
            }
            else
            {
                Modalities = Modality.Text & Modality.Audio;
            }

            FrequencyPenalty = frequencyPenalty;
            LogitBias = logitBias;
            MaxCompletionTokens = maxTokens;
            Number = number;
            PresencePenalty = presencePenalty;

            if (jsonSchema != null)
            {
                ResponseFormatObject = jsonSchema;
            }
            else
            {
                ResponseFormatObject = responseFormat switch
                {
                    ChatResponseFormat.Text or ChatResponseFormat.Json or ChatResponseFormat.JsonSchema => responseFormat,
                    _ => null
                };
            }

            Seed = seed;
            Stops = stops;
            Temperature = temperature;
            TopP = topP;
            LogProbs = topLogProbs.HasValue ? topLogProbs.Value > 0 : null;
            TopLogProbs = topLogProbs;
            ParallelToolCalls = parallelToolCalls;
            User = user;
        }

        /// <summary>
        /// The messages to generate chat completions for, in the chat format.
        /// </summary>
        [Preserve]
        [JsonProperty("messages")]
        public IReadOnlyList<Message> Messages { get; }

        /// <summary>
        /// ID of the model to use.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// Whether or not to store the output of this chat completion request for use in our model distillation or evals products.
        /// </summary>
        [Preserve]
        [JsonProperty("store")]
        public bool? Store { get; set; }

        /// <summary>
        /// Constrains the effort of reasoning for <see href="https://platform.openai.com/docs/guides/reasoning">Reasoning Models</see>.<br/>
        /// Currently supported values are: Low, Medium, High. Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning response.
        /// </summary>
        /// <remarks>
        /// <b>Reasoning models only!</b>
        /// </remarks>
        [Preserve]
        [JsonProperty("reasoning_effort", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReasoningEffort? ReasoningEffort { get; }

        /// <summary>
        /// Developer-defined tags and values used for filtering completions in the dashboard.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.<br/>
        /// Defaults to 0
        /// </summary>
        [Preserve]
        [JsonProperty("frequency_penalty")]
        public double? FrequencyPenalty { get; }

        /// <summary>
        /// Modify the likelihood of specified tokens appearing in the completion.
        /// Accepts a json object that maps tokens(specified by their token ID in the tokenizer)
        /// to an associated bias value from -100 to 100. Mathematically, the bias is added to the logits
        /// generated by the model prior to sampling.The exact effect will vary per model, but values between
        /// -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result
        /// in a ban or exclusive selection of the relevant token.<br/>
        /// Defaults to null
        /// </summary>
        [Preserve]
        [JsonProperty("logit_bias")]
        public IReadOnlyDictionary<string, double> LogitBias { get; }

        /// <summary>
        /// Whether to return log probabilities of the output tokens or not.
        /// If true, returns the log probabilities of each output token returned in the content of message.
        /// </summary>
        /// <remarks>
        /// This option is currently not available on the gpt-4-vision-preview model.
        /// </remarks>
        [Preserve]
        [JsonProperty("logprobs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? LogProbs { get; }

        /// <summary>
        /// An integer between 0 and 5 specifying the number of most likely tokens to return at each token position,
        /// each with an associated log probability.
        /// </summary>
        /// <remarks>
        /// <see cref="LogProbs"/> must be set to true if this parameter is used.
        /// </remarks>
        [Preserve]
        [JsonProperty("top_logprobs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TopLogProbs { get; }

        /// <summary>
        /// The maximum number of tokens allowed for the generated answer.
        /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
        /// </summary>
        /// <remarks>
        /// Still used for older models that support it.
        /// </remarks>
        [JsonProperty("max_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [Obsolete("Use MaxCompletionTokens instead")]
        public int? MaxTokens { get; set; }

        /// <summary>
        /// An upper bound for the number of tokens that can be generated for a completion, including visible output tokens and reasoning tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("max_completion_tokens", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MaxCompletionTokens { get; }

        /// <summary>
        /// How many chat completion choices to generate for each input message.<br/>
        /// Defaults to 1
        /// </summary>
        [Preserve]
        [JsonProperty("n", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Number { get; }

        [Preserve]
        [JsonProperty("modalities")]
        [JsonConverter(typeof(ModalityConverter))]
        public Modality Modalities { get; }

        /// <summary>
        /// Configuration for a Predicted Output, which can greatly improve response times when large parts of the model response are known ahead of time.
        /// This is most common when you are regenerating a file with only minor changes to most of the content.
        /// </summary>
        [Preserve]
        [JsonProperty("prediction", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Prediction { get; set; }

        /// <summary>
        /// Parameters for audio output.
        /// Required when audio output is requested with modalities: ["audio"].
        /// </summary>
        [Preserve]
        [JsonProperty("audio", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public AudioConfig AudioConfig { get; }

        /// <summary>
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on whether they appear in the text so far,
        /// increasing the model's likelihood to talk about new topics.<br/>
        /// Defaults to 0
        /// </summary>
        [Preserve]
        [JsonProperty("presence_penalty", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? PresencePenalty { get; }

        /// <summary>
        /// An object specifying the format that the model must output.
        /// Setting to <see cref="ChatResponseFormat.Json"/> or <see cref="ChatResponseFormat.JsonSchema"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.
        /// </summary>
        /// <remarks>
        /// Important: When using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.
        /// Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit,
        /// resulting in a long-running and seemingly "stuck" request. Also note that the message content may be partially cut off if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </remarks>
        [Preserve]
        [JsonConverter(typeof(ResponseFormatConverter))]
        [JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseFormatObject ResponseFormatObject { get; internal set; }

        /// <summary>
        /// An object specifying the format that the model must output.
        /// Setting to <see cref="ChatResponseFormat.Json"/> or <see cref="ChatResponseFormat.JsonSchema"/> enables JSON mode,
        /// which guarantees the message the model generates is valid JSON.
        /// </summary>
        /// <remarks>
        /// Important: When using JSON mode, you must also instruct the model to produce JSON yourself via a system or user message.
        /// Without this, the model may generate an unending stream of whitespace until the generation reaches the token limit,
        /// resulting in a long-running and seemingly "stuck" request. Also note that the message content may be partially cut off if finish_reason="length",
        /// which indicates the generation exceeded max_tokens or the conversation exceeded the max context length.
        /// </remarks>
        [Preserve]
        [JsonIgnore]
        public ChatResponseFormat ResponseFormat => ResponseFormatObject ?? ChatResponseFormat.Auto;

        /// <summary>
        /// This feature is in Beta. If specified, our system will make a best effort to sample deterministically,
        /// such that repeated requests with the same seed and parameters should return the same result.
        /// Determinism is not guaranteed, and you should refer to the system_fingerprint response parameter to
        /// monitor changes in the backend.
        /// </summary>
        [Preserve]
        [JsonProperty("seed", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Seed { get; }

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
        /// Up to 4 sequences where the API will stop generating further tokens.
        /// </summary>
        [Preserve]
        [JsonProperty("stop", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] Stops { get; }

        /// <summary>
        /// Specifies where the results should stream and be returned at one time.
        /// Do not set this yourself, use the appropriate methods on <see cref="ChatEndpoint"/> instead.<br/>
        /// Defaults to false
        /// </summary>
        [Preserve]
        [JsonProperty("stream")]
        public bool Stream { get; internal set; }

        [Preserve]
        [JsonProperty("stream_options", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public StreamOptions StreamOptions { get; internal set; }

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
        /// A list of tools the model may call. Currently, only functions are supported as a tool.
        /// Use this to provide a list of functions the model may generate JSON inputs for.
        /// </summary>
        [Preserve]
        [JsonProperty("tools", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// Controls which (if any) function is called by the model.<br/>
        /// 'none' means the model will not call a function and instead generates a message.&lt;br/&gt;
        /// 'auto' means the model can pick between generating a message or calling a function.&lt;br/&gt;
        /// Specifying a particular function via {"type: "function", "function": {"name": "my_function"}}
        /// forces the model to call that function.<br/>
        /// 'none' is the default when no functions are present.<br/>
        /// 'auto' is the default if functions are present.<br/>
        /// </summary>
        [Preserve]
        [JsonProperty("tool_choice", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object ToolChoice { get; }

        /// <summary>
        /// Whether to enable parallel function calling during tool use.
        /// </summary>
        [Preserve]
        [JsonProperty("parallel_tool_calls", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ParallelToolCalls { get; }

        /// <summary>
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </summary>
        [Preserve]
        [JsonProperty("user")]
        public string User { get; }

        /// <inheritdoc />
        [Preserve]
        public override string ToString() => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);
    }
}
