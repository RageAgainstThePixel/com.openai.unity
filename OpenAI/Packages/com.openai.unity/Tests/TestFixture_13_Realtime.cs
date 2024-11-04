// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Models;
using OpenAI.Realtime;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_13_Realtime : AbstractTestFixture
    {
        [Test]
        public async Task Test_01_RealtimeSession()
        {
            try
            {
                Assert.IsNotNull(OpenAIClient.RealtimeEndpoint);
                var sessionCreatedTcs = new TaskCompletionSource<SessionResponse>(new CancellationTokenSource(500));
                var sessionOptions = new SessionResource(Model.GPT4oRealtime);
                using var session = await OpenAIClient.RealtimeEndpoint.CreateSessionAsync(sessionOptions, OnRealtimeEvent);

                try
                {
                    Assert.IsNotNull(session);
                    session.OnEventReceived += OnRealtimeEvent;
                }
                finally
                {
                    session.OnEventReceived -= OnRealtimeEvent;
                }

                await sessionCreatedTcs.Task;

                void OnRealtimeEvent(IRealtimeEvent @event)
                {
                    switch (@event)
                    {
                        case SessionResponse sessionResponse:
                            sessionCreatedTcs.SetResult(sessionResponse);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
