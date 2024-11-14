// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.Async;

namespace OpenAI.Realtime
{
    public sealed class RealtimeEndpoint : OpenAIBaseEndpoint
    {
        public RealtimeEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "realtime";

        protected override bool? IsWebSocketEndpoint => true;

        /// <summary>
        /// Creates a new realtime session with the provided <see cref="Options"/> options.
        /// </summary>
        /// <param name="options"><see cref="Options"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="RealtimeSession"/>.</returns>
        public async Task<RealtimeSession> CreateSessionAsync(Options options = null, CancellationToken cancellationToken = default)
        {
            string model = string.IsNullOrWhiteSpace(options?.Model) ? Model.GPT4oRealtime : options!.Model;
            var queryParameters = new Dictionary<string, string>();

            if (client.Settings.Info.IsAzureOpenAI)
            {
                queryParameters["deployment"] = model;
            }
            else
            {
                queryParameters["model"] = model;
            }

            var session = new RealtimeSession(client.CreateWebSocket(GetUrl(queryParameters: queryParameters)), EnableDebug);
            var sessionCreatedTcs = new TaskCompletionSource<SessionResponse>();

            try
            {
                session.OnEventReceived += OnEventReceived;
                session.OnError += OnError;
                await session.ConnectAsync(cancellationToken).ConfigureAwait(true);
                var sessionResponse = await sessionCreatedTcs.Task.WithCancellation(cancellationToken).ConfigureAwait(true);
                session.Options = sessionResponse.Options;
                await session.SendAsync(new UpdateSessionRequest(options), cancellationToken: cancellationToken).ConfigureAwait(true);
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
