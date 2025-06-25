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
using OpenAI.Realtime;
using OpenAI.Responses;
using OpenAI.Threads;
using OpenAI.VectorStores;
using System.Collections.Generic;
using System.Security.Authentication;
using Utilities.WebRequestRest;
using Utilities.WebSockets;

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
            RealtimeEndpoint = new RealtimeEndpoint(this);
            ResponsesEndpoint = new ResponsesEndpoint(this);
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
                new StringEnumConverter(new SnakeCaseNamingStrategy()),
                new RealtimeClientEventConverter(),
                new RealtimeServerEventConverter(),
                new ResponseItemConverter(),
                new ResponseContentConverter(),
                new AnnotationConverter(),
            }
        };

        internal static JsonSerializer JsonSerializer { get; } = JsonSerializer.Create(JsonSerializationOptions);

        #region Endpoints

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
        public AudioEndpoint AudioEndpoint { get; }

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

        /// <summary>
        /// Communicate with a GPT-4o class model in real time using WebSockets.
        /// Supports text and audio inputs and outputs, along with audio transcriptions.
        /// <see href="https://platform.openai.com/docs/api-reference/realtime"/>
        /// </summary>
        public RealtimeEndpoint RealtimeEndpoint { get; }

        /// <summary>
        /// Creates a model response.
        /// Provide text or image inputs to generate text or JSON outputs.
        /// Have the model call your own custom code or use built-in tools like web search or file search to use your own data as input for the model's response.
        /// <see href="https://platform.openai.com/docs/api-reference/responses"/>
        /// </summary>
        public ResponsesEndpoint ResponsesEndpoint { get; }

        #endregion Endpoints

        internal WebSocket CreateWebSocket(string url)
        {
            return new WebSocket(url, new Dictionary<string, string>
            {
#if !PLATFORM_WEBGL
                { "User-Agent", "OpenAI-DotNet" },
                { "OpenAI-Beta", "realtime=v1" },
                { "Authorization", $"Bearer {Authentication.Info.ApiKey}" }
#endif
            }, new List<string>
            {
#if PLATFORM_WEBGL // Web browsers do not support headers. https://github.com/openai/openai-realtime-api-beta/blob/339e9553a757ef1cf8c767272fc750c1e62effbb/lib/api.js#L76-L80
                "realtime",
                $"openai-insecure-api-key.{Authentication.Info.ApiKey}",
                "openai-beta.realtime-v1"
#endif
            });
        }
    }
}
