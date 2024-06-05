// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenAI.Assistants;
using OpenAI.Audio;
using OpenAI.Batch;
using OpenAI.Chat;
using OpenAI.Embeddings;
using OpenAI.Files;
using OpenAI.FineTuning;
using OpenAI.Images;
using OpenAI.Models;
using OpenAI.Moderations;
using OpenAI.Threads;
using OpenAI.VectorStores;
using System.Collections.Generic;
using System.Security.Authentication;
using Utilities.WebRequestRest;

namespace OpenAI
{
    /// <summary>
    /// Entry point to the OpenAI API, handling auth and allowing access to the various API endpoints
    /// </summary>
    public sealed class OpenAIClient : BaseClient<OpenAIAuthentication, OpenAISettings>
    {
        /// <inheritdoc />
        public OpenAIClient(OpenAIConfiguration configuration)
            : this(
                configuration != null ? new OpenAIAuthentication(configuration) : null,
                configuration != null ? new OpenAISettings(configuration) : null)
        {
        }

        /// <summary>
        /// Creates a new entry point to the OpenAPI API, handling auth and allowing access to the various API endpoints
        /// </summary>
        /// <param name="authentication">The API authentication information to use for API calls,
        /// or <see langword="null"/> to attempt to use the <see cref="OpenAIAuthentication.Default"/>,
        /// potentially loading from environment vars or from a config file.</param>
        /// <param name="settings">
        /// Optional, <see cref="OpenAISettings"/> for specifying OpenAI deployments to Azure or proxy domain.
        /// </param>
        /// <exception cref="AuthenticationException">Raised when authentication details are missing or invalid.</exception>
        public OpenAIClient(OpenAIAuthentication authentication = null, OpenAISettings settings = null)
            : base(authentication ?? OpenAIAuthentication.Default, settings ?? OpenAISettings.Default)
        {
            ModelsEndpoint = new ModelsEndpoint(this);
            ChatEndpoint = new ChatEndpoint(this);
            ImagesEndPoint = new ImagesEndpoint(this);
            EmbeddingsEndpoint = new EmbeddingsEndpoint(this);
            AudioEndpoint = new AudioEndpoint(this);
            FilesEndpoint = new FilesEndpoint(this);
            FineTuningEndpoint = new FineTuningEndpoint(this);
            ModerationsEndpoint = new ModerationsEndpoint(this);
            ThreadsEndpoint = new ThreadsEndpoint(this);
            AssistantsEndpoint = new AssistantsEndpoint(this);
            BatchEndpoint = new BatchEndpoint(this);
            VectorStoresEndpoint = new VectorStoresEndpoint(this);
        }

        protected override void SetupDefaultRequestHeaders()
        {
            var headers = new Dictionary<string, string>
            {
#if !UNITY_WEBGL
                { "User-Agent", "com.openai.unity" },
#endif
                { "OpenAI-Beta", "assistants=v2" }
            };

            if (Settings.Info.BaseRequestUrlFormat.Contains(OpenAISettingsInfo.OpenAIDomain) &&
                (string.IsNullOrWhiteSpace(Authentication.Info.ApiKey) ||
                 (!Authentication.Info.ApiKey.Contains(OpenAIAuthInfo.SecretKeyPrefix) &&
                  !Authentication.Info.ApiKey.Contains(OpenAIAuthInfo.SessionKeyPrefix))))
            {
                throw new InvalidCredentialException($"{nameof(Authentication.Info.ApiKey)} must start with '{OpenAIAuthInfo.SecretKeyPrefix}'");
            }

            if (Settings.Info.UseOAuthAuthentication)
            {
                headers.Add("Authorization", Rest.GetBearerOAuthToken(Authentication.Info.ApiKey));
            }
            else
            {
                headers.Add("api-key", Authentication.Info.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(Authentication?.Info?.OrganizationId))
            {
                headers.Add("OpenAI-Organization", Authentication.Info.OrganizationId);
            }

            if (!string.IsNullOrWhiteSpace(Authentication?.Info?.ProjectId))
            {
                headers.Add("OpenAI-Project", Authentication.Info.ProjectId);
            }

            DefaultRequestHeaders = headers;
        }

        protected override void ValidateAuthentication()
        {
            if (Authentication?.Info == null)
            {
                throw new InvalidCredentialException($"Invalid {nameof(OpenAIAuthentication)}");
            }

            if (!HasValidAuthentication)
            {
                throw new InvalidCredentialException($"Missing API key for {nameof(OpenAIClient)}");
            }
        }

        public override bool HasValidAuthentication => !string.IsNullOrWhiteSpace(Authentication?.Info?.ApiKey);

        /// <summary>
        /// The <see cref="JsonSerializationOptions"/> to use when making calls to the API.
        /// </summary>
        internal static JsonSerializerSettings JsonSerializationOptions { get; } = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(new SnakeCaseNamingStrategy())
            }
        };

        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand which models are available for certain endpoints: <see href="https://platform.openai.com/docs/models/model-endpoint-compatibility"/>.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/models"/>
        /// </summary>
        public ModelsEndpoint ModelsEndpoint { get; }

        /// <summary>
        /// Given a chat conversation, the model will return a chat completion response.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/chat"/>
        /// </summary>
        public ChatEndpoint ChatEndpoint { get; }

        /// <summary>
        /// Given a prompt and/or an input image, the model will generate a new image.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/images"/>
        /// </summary>
        public ImagesEndpoint ImagesEndPoint { get; }

        /// <summary>
        /// Get a vector representation of a given input that can be easily consumed by machine learning models and algorithms.<br/>
        /// <see href="https://platform.openai.com/docs/guides/embeddings"/>
        /// </summary>
        public EmbeddingsEndpoint EmbeddingsEndpoint { get; }

        /// <summary>
        /// Transforms audio into text.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/audio"/>
        /// </summary>
        public AudioEndpoint AudioEndpoint { get; set; }

        /// <summary>
        /// Files are used to upload documents that can be used with features like Assistants, Fine-tuning, and Batch API.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/files"/>
        /// </summary>
        public FilesEndpoint FilesEndpoint { get; }

        /// <summary>
        /// Manage fine-tuning jobs to tailor a model to your specific training data.<br/>
        /// <see href="https://platform.openai.com/docs/guides/fine-tuning"/>
        /// </summary>
        public FineTuningEndpoint FineTuningEndpoint { get; }

        /// <summary>
        /// The moderation endpoint is a tool you can use to check whether content complies with OpenAI's content policy.
        /// Developers can thus identify content that our content policy prohibits and take action, for instance by filtering it.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/moderations"/>
        /// </summary>
        public ModerationsEndpoint ModerationsEndpoint { get; }

        /// <summary>
        /// Create threads that assistants can interact with.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/threads"/>
        /// </summary>
        public ThreadsEndpoint ThreadsEndpoint { get; }

        /// <summary>
        /// Build assistants that can call models and use tools to perform tasks.<br/>
        /// <see href="https://platform.openai.com/docs/api-reference/assistants"/>
        /// </summary>
        public AssistantsEndpoint AssistantsEndpoint { get; }

        /// <summary>
        /// Create large batches of API requests for asynchronous processing.
        /// The Batch API returns completions within 24 hours for a 50% discount.
        /// <see href="https://platform.openai.com/docs/api-reference/batch"/>
        /// </summary>
        public BatchEndpoint BatchEndpoint { get; }

        /// <summary>
        /// Vector stores are used to store files for use by the file_search tool.
        /// <see href="https://platform.openai.com/docs/api-reference/vector-stores"/>
        /// </summary>
        public VectorStoresEndpoint VectorStoresEndpoint { get; }
    }
}
