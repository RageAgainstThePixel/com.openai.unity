using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CreateThreadAndRunRequest
    {
        [Preserve]
        public CreateThreadAndRunRequest(string assistantId, CreateThreadAndRunRequest request)
            : this(assistantId, request?.Model, request?.Instructions, request?.Tools, request?.Metadata, request?.Temperature)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assistantId">
        /// The ID of the assistant to use to execute this run.
        /// </param>
        /// <param name="model">
        /// The ID of the Model to be used to execute this run.
        /// If a value is provided here, it will override the model associated with the assistant.
        /// If not, the model associated with the assistant will be used.
        /// </param>
        /// <param name="instructions">
        /// Override the default system message of the assistant.
        /// This is useful for modifying the behavior on a per-run basis.
        /// </param>
        /// <param name="tools">
        /// Override the tools the assistant can use for this run.
        /// This is useful for modifying the behavior on a per-run basis.
        /// </param>
        /// <param name="metadata">
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </param>
        /// <param name="temperature">
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output
        /// more random, while lower values like 0.2 will make it more focused and deterministic.
        /// When null the default temperature (1) will be used.
        /// </param>
        /// <param name="createThreadRequest">
        /// Optional, <see cref="CreateThreadRequest"/>.
        /// </param>
        [Preserve]
        public CreateThreadAndRunRequest(string assistantId, string model = null, string instructions = null, IReadOnlyList<Tool> tools = null, IReadOnlyDictionary<string, string> metadata = null, float? temperature = null, CreateThreadRequest createThreadRequest = null)
        {
            AssistantId = assistantId;
            Model = model;
            Instructions = instructions;
            Tools = tools;
            Metadata = metadata;
            Temperature = temperature;
            ThreadRequest = createThreadRequest;
        }

        /// <summary>
        /// The ID of the assistant to use to execute this run.
        /// </summary>
        [Preserve]
        [JsonProperty("assistant_id")]
        public string AssistantId { get; }

        /// <summary>
        /// The ID of the Model to be used to execute this run.
        /// If a value is provided here, it will override the model associated with the assistant.
        /// If not, the model associated with the assistant will be used.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        /// <summary>
        /// Override the default system message of the assistant.
        /// This is useful for modifying the behavior on a per-run basis.
        /// </summary>
        [Preserve]
        [JsonProperty("instructions")]
        public string Instructions { get; }

        /// <summary>
        /// Override the tools the assistant can use for this run.
        /// This is useful for modifying the behavior on a per-run basis.
        /// </summary>
        [Preserve]
        [JsonProperty("tools")]
        public IReadOnlyList<Tool> Tools { get; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [Preserve]
        [JsonProperty("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; }

        /// <summary>
        /// What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output
        /// more random, while lower values like 0.2 will make it more focused and deterministic.
        /// When null the default temperature (1) will be used.
        /// </summary>
        [Preserve]
        [JsonProperty("temperature")]
        public float? Temperature { get; }

        [Preserve]
        [JsonProperty("thread")]
        public CreateThreadRequest ThreadRequest { get; }
    }
}
