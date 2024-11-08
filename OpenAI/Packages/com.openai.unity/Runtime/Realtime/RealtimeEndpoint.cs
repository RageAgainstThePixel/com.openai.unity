// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Models;
using System;
using System.Collections.Generic;
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

        public async Task<RealtimeSession> CreateSessionAsync(SessionResource options = null, Action<IRealtimeEvent> sessionEvents = null, CancellationToken cancellationToken = default)
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
                await sessionCreatedTcs.Task.WithCancellation(cancellationToken).ConfigureAwait(true);
                await session.SendAsync(new UpdateSessionRequest(options), cancellationToken).ConfigureAwait(true);
            }
            finally
            {
                session.OnEventReceived -= OnEventReceived;
                session.OnError -= OnError;
            }

            return session;

            void OnError(Error error)
            {
                sessionCreatedTcs.SetException(error.Exception ?? new Exception(error.Message));
            }

            void OnEventReceived(IRealtimeEvent @event)
            {
                try
                {
                    switch (@event)
                    {
                        case SessionResponse sessionResponse:
                            sessionCreatedTcs.SetResult(sessionResponse);
                            break;
                        case RealtimeEventError realtimeEventError:
                            sessionCreatedTcs.SetException(new Exception(realtimeEventError.Error.Message));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    sessionCreatedTcs.SetException(e);
                }
                finally
                {
                    sessionEvents?.Invoke(@event);
                }
            }
        }
    }
}
