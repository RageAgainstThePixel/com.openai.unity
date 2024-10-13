// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Realtime
{
    public sealed class RealtimeEndpoint : OpenAIBaseEndpoint
    {
        public RealtimeEndpoint(OpenAIClient client) : base(client) { }

        protected override string Root => "realtime";

        protected override bool? IsWebSocketEndpoint => true;

        public async Task<RealtimeSession> CreateSessionAsync(SessionResource options = null, Action<IRealtimeEvent> sessionEvents = null, CancellationToken cancellationToken = default)
        {
            var model = string.IsNullOrWhiteSpace(options?.Model) ? Model.GPT4oRealtime : options!.Model;
            var queryParameters = new Dictionary<string, string>();

            if (client.Settings.Info.IsAzureOpenAI)
            {
                queryParameters["deployment"] = model;
            }
            else
            {
                queryParameters["model"] = model;
            }

            var session = new RealtimeSession(client.CreateWebSocket(GetUrl(queryParameters: queryParameters)));
            var sessionCreatedTcs = new TaskCompletionSource<SessionResponse>(new CancellationTokenSource(500));

            try
            {
                session.OnEventReceived += OnEventReceived;
                await session.ConnectAsync();
                await sessionCreatedTcs.Task;
            }
            finally
            {
                session.OnEventReceived -= OnEventReceived;
            }

            return session;

            void OnEventReceived(IRealtimeEvent @event)
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

                sessionEvents?.Invoke(@event);
            }
        }
    }
}
