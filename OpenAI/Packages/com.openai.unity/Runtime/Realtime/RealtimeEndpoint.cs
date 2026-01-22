// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.Async;
using Utilities.WebRequestRest;
using Utilities.WebSockets;

namespace OpenAI.Realtime
{
    public sealed class RealtimeEndpoint : OpenAIBaseEndpoint
    {
        public RealtimeEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "realtime";

        /// <summary>
        /// Creates a new realtime session with the provided <see cref="SessionConfiguration"/> options.
        /// </summary>
        /// <param name="configuration"><see cref="SessionConfiguration"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RealtimeSession"/>.</returns>
        public Task<RealtimeSession> CreateSessionAsync(SessionConfiguration configuration = null, CancellationToken cancellationToken = default)
            => CreateSessionAsync(configuration, GetDefaultApiKey(), cancellationToken);

        /// <summary>
        /// Creates a new realtime session with the provided <see cref="SessionConfiguration"/> options and auth token.
        /// </summary>
        /// <param name="configuration"><see cref="SessionConfiguration"/>.</param>
        /// <param name="apiKeyOverride">Parent API key or ephemeral key.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RealtimeSession"/>.</returns>
        public async Task<RealtimeSession> CreateSessionAsync(SessionConfiguration configuration, string apiKeyOverride, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(apiKeyOverride))
            {
                throw new AuthenticationException("Missing API key or ephemeral token.");
            }

            string model = string.IsNullOrWhiteSpace(configuration?.Model) ? Model.GPT_Realtime : configuration!.Model;
            var queryParameters = new Dictionary<string, string>();

            if (client.Settings.Info.IsAzureOpenAI)
            {
                queryParameters["deployment"] = model;
            }
            else
            {
                queryParameters["model"] = model;
            }

            var websocket = new WebSocket(
                GetWebsocketUri(queryParameters: queryParameters),
                BuildRealtimeHeaders(apiKeyOverride),
                BuildRealtimeProtocols(apiKeyOverride));
            var session = new RealtimeSession(websocket, EnableDebug);
            var sessionCreatedTcs = new TaskCompletionSource<SessionResponse>();

            try
            {
                session.OnEventReceived += OnEventReceived;
                session.OnError += OnError;
                await session.ConnectAsync(cancellationToken).ConfigureAwait(true);
                var sessionResponse = await sessionCreatedTcs.Task.WithCancellation(cancellationToken).ConfigureAwait(true);
                session.Configuration = sessionResponse.SessionConfiguration;

                if (configuration != null)
                {
                    await session.SendAsync(new UpdateSessionRequest(configuration), cancellationToken).ConfigureAwait(true);
                }
            }
            finally
            {
                session.OnError -= OnError;
                session.OnEventReceived -= OnEventReceived;
            }

            return session;

            void OnError(Exception e)
                => sessionCreatedTcs.TrySetException(e);

            void OnEventReceived(IRealtimeEvent @event)
            {
                try
                {
                    switch (@event)
                    {
                        case SessionResponse sessionResponse:
                            if (sessionResponse.Type == "session.created")
                            {
                                sessionCreatedTcs.TrySetResult(sessionResponse);
                            }

                            break;
                        case RealtimeEventError realtimeEventError:
                            sessionCreatedTcs.TrySetException(realtimeEventError.Error.Code is "invalid_session_token" or "invalid_api_key"
                                ? new AuthenticationException(realtimeEventError.Error.Message)
                                : new Exception(realtimeEventError.Error.Message));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    sessionCreatedTcs.TrySetException(e);
                }
            }
        }

        /// <summary>
        /// Creates a realtime client secret for use in client environments.
        /// </summary>
        /// <param name="configuration">Optional session configuration to bind to the client secret.</param>
        /// <param name="expiresAfter">Optional expiration settings for the client secret.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="ClientSecretResponse"/>.</returns>
        public async Task<ClientSecretResponse> CreateClientSecretAsync(
            SessionConfiguration configuration = null,
            ExpiresAfter expiresAfter = null,
            CancellationToken cancellationToken = default)
        {
            var request = new ClientSecretRequest(
                configuration,
                expiresAfter ?? configuration?.ExpiresAfter);
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/client_secrets"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Deserialize<ClientSecretResponse>(client);
        }

        private Dictionary<string, string> BuildRealtimeHeaders(string apiKeyOverride)
        {
            var headers = new Dictionary<string, string>();
#if !PLATFORM_WEBGL
            headers["User-Agent"] = "com.openai.unity";

            if (client.Settings.Info.UseOAuthAuthentication)
            {
                headers["Authorization"] = Rest.GetBearerOAuthToken(apiKeyOverride);
            }
            else
            {
                headers["api-key"] = apiKeyOverride;
            }

            if (client.DefaultRequestHeaders.TryGetValue("OpenAI-Organization", out var organizationId) &&
                !string.IsNullOrWhiteSpace(organizationId))
            {
                headers["OpenAI-Organization"] = organizationId;
            }

            if (client.DefaultRequestHeaders.TryGetValue("OpenAI-Project", out var projectId) &&
                !string.IsNullOrWhiteSpace(projectId))
            {
                headers["OpenAI-Project"] = projectId;
            }
#endif
            return headers;
        }

        private List<string> BuildRealtimeProtocols(string apiKeyOverride)
        {
            return new List<string>
            {
#if PLATFORM_WEBGL // Web browsers do not support headers.
                "realtime",
                $"openai-insecure-api-key.{apiKeyOverride}"
#endif
            };
        }

        private string GetDefaultApiKey()
        {
            if (client.DefaultRequestHeaders != null &&
                client.DefaultRequestHeaders.TryGetValue("Authorization", out var authorization) &&
                !string.IsNullOrWhiteSpace(authorization))
            {
                const string bearerPrefix = "Bearer ";
                return authorization.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                    ? authorization.Substring(bearerPrefix.Length).Trim()
                    : authorization;
            }

            if (client.DefaultRequestHeaders != null &&
                client.DefaultRequestHeaders.TryGetValue("api-key", out var apiKey) &&
                !string.IsNullOrWhiteSpace(apiKey))
            {
                return apiKey;
            }

            return null;
        }
    }
}
