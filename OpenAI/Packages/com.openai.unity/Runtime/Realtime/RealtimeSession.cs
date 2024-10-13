// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.WebSockets;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeSession : IDisposable
    {
        public event Action<IRealtimeEvent> OnEventReceived;

        private readonly WebSocket websocketClient;

        internal RealtimeSession(WebSocket wsClient)
        {
            websocketClient = wsClient;
            websocketClient.OnMessage += OnMessage;
        }

        private void OnMessage(DataFrame dataFrame)
        {
            if (dataFrame.Type == OpCode.Text)
            {
                Debug.Log($"[dataframe] {dataFrame.Text}");

                try
                {
                    var @event = JsonConvert.DeserializeObject<IRealtimeEvent>(dataFrame.Text, OpenAIClient.JsonSerializationOptions);
                    OnEventReceived?.Invoke(@event);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        ~RealtimeSession() => Dispose(false);

        #region IDisposable

        private bool isDisposed;

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

        internal async Task ConnectAsync()
        {
            var connectTcs = new TaskCompletionSource<State>();
            websocketClient.OnOpen += OnWebsocketClientOnOnOpen;
            websocketClient.OnError += OnWebsocketClientOnOnError;

            try
            {
                websocketClient.Connect();
                await connectTcs.Task;

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
    }
}
