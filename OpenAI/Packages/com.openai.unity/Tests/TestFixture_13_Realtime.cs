// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Models;
using OpenAI.Realtime;
using System;
using System.Collections.Generic;
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
            RealtimeSession session = null;

            try
            {
                Assert.IsNotNull(OpenAIClient.RealtimeEndpoint);
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

                var tools = new List<Tool>
                {
                    Tool.FromFunc("goodbye", () =>
                    {
                        cts.Cancel();
                        return "goodbye!";
                    })
                };

                var sessionOptions = new SessionResource(
                    Model.GPT4oRealtime,
                    tools: tools);

                session = await OpenAIClient.RealtimeEndpoint.CreateSessionAsync(sessionOptions, cts.Token);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.Options);
                Assert.AreEqual(sessionOptions.Model, session.Options.Model);

                var tasks = new List<Task>
                {
                    SendResponses(session),
                    session.ReceiveUpdatesAsync<IServerEvent>(SessionEvents, cts.Token)
                };

                async Task SendResponses(RealtimeSession s)
                {
                    await s.SendAsync(new ConversationItemCreateRequest("Hello!"), cts.Token);
                    await s.SendAsync(new ResponseCreateRequest(), cts.Token);
                    await Task.Delay(5000, cts.Token).ConfigureAwait(true);
                    await s.SendAsync(new ConversationItemCreateRequest("Goodbye!"), cts.Token);
                    await s.SendAsync(new ResponseCreateRequest(), cts.Token);
                }

                void SessionEvents(IServerEvent @event)
                {
                    switch (@event)
                    {
                        case ResponseAudioTranscriptResponse transcriptResponse:
                            Debug.Log(transcriptResponse.ToString());
                            break;
                        case ResponseFunctionCallArguments functionCallResponse:
                            if (functionCallResponse.IsDone)
                            {
                                ToolCall toolCall = functionCallResponse;
                                toolCall.InvokeFunction();
                            }

                            break;
                    }
                }

                await Task.WhenAll(tasks).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
            finally
            {
                session?.Dispose();
            }
        }
    }
}
