// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Async;
using Utilities.WebSockets;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeSession : IDisposable
    {
        [Preserve]
        public event Action<IServerEvent> OnEventReceived;

        [Preserve]
        public event Action<IClientEvent> OnEventSent;

        [Preserve]
        public event Action<Error> OnError;

        private readonly WebSocket websocketClient;

        [Preserve]
        public bool EnableDebug { get; set; }

        [Preserve]
        public int EventTimeout { get; set; } = 30;

        [Preserve]
        internal RealtimeSession(WebSocket wsClient, bool enableDebug)
        {
            websocketClient = wsClient;
            EnableDebug = enableDebug;
            websocketClient.OnMessage += OnMessage;
        }

        [Preserve]
        private void OnMessage(DataFrame dataFrame)
        {
            if (dataFrame.Type == OpCode.Text)
            {
                if (EnableDebug)
                {
                    Debug.Log(dataFrame.Text);
                }

                try
                {
                    var @event = JsonConvert.DeserializeObject<IServerEvent>(dataFrame.Text, OpenAIClient.JsonSerializationOptions);
                    OnEventReceived?.Invoke(@event);
                }
                catch (Exception e)
                {
                    OnError?.Invoke(new Error(e));
                }
            }
        }

        [Preserve]
        ~RealtimeSession() => Dispose(false);

        #region IDisposable

        private bool isDisposed;

        [Preserve]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                websocketClient.OnMessage -= OnMessage;
                websocketClient.Dispose();
                isDisposed = true;
            }
        }

        #endregion IDisposable

        [Preserve]
        internal async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            var connectTcs = new TaskCompletionSource<State>();
            websocketClient.OnOpen += OnWebsocketClientOnOnOpen;
            websocketClient.OnError += OnWebsocketClientOnOnError;

            try
            {
                // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                // don't call async because it is blocking until connection is closed.
                websocketClient.Connect();
                await connectTcs.Task.WithCancellation(cancellationToken).ConfigureAwait(true);

                if (websocketClient.State != State.Open)
                {
                    throw new Exception($"Failed to start new session! {websocketClient.State}");
                }
            }
            finally
            {
                websocketClient.OnOpen -= OnWebsocketClientOnOnOpen;
                websocketClient.OnError -= OnWebsocketClientOnOnError;
            }

            return;

            void OnWebsocketClientOnOnError(Exception e)
                => connectTcs.TrySetException(e);
            void OnWebsocketClientOnOnOpen()
                => connectTcs.TrySetResult(websocketClient.State);
        }

        [Preserve]
        public async Task<IServerEvent> SendAsync<T>(T clientEvent, Action<IServerEvent> sessionEvents = null, CancellationToken cancellationToken = default)
            where T : IClientEvent
        {
            if (websocketClient.State != State.Open)
            {
                throw new Exception($"Websocket connection is not open! {websocketClient.State}");
            }

            var payload = clientEvent.ToJsonString();

            if (EnableDebug)
            {
                Debug.Log(payload);
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(EventTimeout));
            using var eventCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
            var tcs = new TaskCompletionSource<IServerEvent>();
            eventCts.Token.Register(() => tcs.TrySetCanceled());
            OnEventReceived += EventCallback;
            OnEventSent?.Invoke(clientEvent);
            await websocketClient.SendAsync(payload, cancellationToken).ConfigureAwait(true);
            return await tcs.Task.WithCancellation(eventCts.Token);

            void EventCallback(IServerEvent serverEvent)
            {
                sessionEvents?.Invoke(serverEvent);

                try
                {
                    if (serverEvent is RealtimeEventError serverError)
                    {
                        Debug.LogWarning($"{clientEvent.Type} -> {serverEvent.Type}");
                        tcs.TrySetException(new Exception(serverError.ToString()));
                        OnEventReceived -= EventCallback;
                        return;
                    }

                    if (clientEvent is UpdateSessionRequest &&
                        serverEvent is SessionResponse)
                    {
                        Complete();
                        return;
                    }

                    if (clientEvent is ConversationItemCreateRequest &&
                        serverEvent is ConversationItemCreatedResponse)
                    {
                        Complete();
                        return;
                    }

                    if (clientEvent is ResponseCreateRequest &&
                        serverEvent is RealtimeResponse response)
                    {
                        if (response.Response.Status == RealtimeResponseStatus.InProgress)
                        {
                            return;
                        }

                        if (response.Response.Status != RealtimeResponseStatus.Completed)
                        {
                            tcs.TrySetException(new Exception(response.Response.StatusDetails.Error.ToString()));
                        }
                        else
                        {
                            Complete();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                void Complete()
                {
                    Debug.LogWarning($"{clientEvent.Type} -> {serverEvent.Type}");
                    tcs.TrySetResult(serverEvent);
                    OnEventReceived -= EventCallback;
                }
            }
        }
    }
}
