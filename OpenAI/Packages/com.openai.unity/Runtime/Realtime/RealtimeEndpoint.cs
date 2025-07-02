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
        public async Task<RealtimeSession> CreateSessionAsync(SessionConfiguration configuration = null, CancellationToken cancellationToken = default)
        {
            string model = string.IsNullOrWhiteSpace(configuration?.Model) ? Model.GPT4oRealtime : configuration!.Model;
            var queryParameters = new Dictionary<string, string>();

            if (client.Settings.Info.IsAzureOpenAI)
            {
                queryParameters["deployment"] = model;
            }
            else
            {
                queryParameters["model"] = model;
            }

            var payload = JsonConvert.SerializeObject(configuration, OpenAIClient.JsonSerializationOptions);
            var createSessionResponse = await Rest.PostAsync(GetUrl("/sessions"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            createSessionResponse.Validate(EnableDebug);
            var createSession = createSessionResponse.Deserialize<SessionConfiguration>(client);

            if (createSession == null ||
                string.IsNullOrWhiteSpace(createSession.ClientSecret?.EphemeralApiKey))
            {
                throw new InvalidOperationException("Failed to create a session. Ensure the configuration is valid and the API key is set.");
            }

            var websocket = new WebSocket(GetWebsocketUri(queryParameters: queryParameters), new Dictionary<string, string>
            {
#if !PLATFORM_WEBGL
                { "User-Agent", "OpenAI-DotNet" },
                { "OpenAI-Beta", "realtime=v1" },
                { "Authorization", $"Bearer {createSession.ClientSecret!.EphemeralApiKey}" }
#endif
            }, new List<string>
            {
#if PLATFORM_WEBGL // Web browsers do not support headers. https://github.com/openai/openai-realtime-api-beta/blob/339e9553a757ef1cf8c767272fc750c1e62effbb/lib/api.js#L76-L80
                "realtime",
                $"openai-insecure-api-key.{createSession.ClientSecret!.EphemeralApiKey}",
                "openai-beta.realtime-v1"
#endif
            });
            var session = new RealtimeSession(websocket, EnableDebug);
            var sessionCreatedTcs = new TaskCompletionSource<SessionResponse>();

            try
            {
                session.OnEventReceived += OnEventReceived;
                session.OnError += OnError;
                await session.ConnectAsync(cancellationToken).ConfigureAwait(true);
                var sessionResponse = await sessionCreatedTcs.Task.WithCancellation(cancellationToken).ConfigureAwait(true);
                session.Configuration = sessionResponse.SessionConfiguration;
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
    }
}
