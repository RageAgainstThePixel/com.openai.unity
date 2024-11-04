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
                await connectTcs.Task.WithCancellation(cancellationToken);

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
        public async Task SendAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IClientEvent
        {
            if (websocketClient.State != State.Open)
            {
                throw new Exception($"Websocket connection is not open! {websocketClient.State}");
            }

            var payload = @event.ToJsonString();

            if (EnableDebug)
            {
                Debug.Log(payload);
            }

            OnEventSent?.Invoke(@event);
            await websocketClient.SendAsync(payload, cancellationToken);
        }
    }
}
