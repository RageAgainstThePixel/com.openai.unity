// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Chat;
using OpenAI.Files;
using OpenAI.FineTuning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Models;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_09_FineTuning
    {
        private async Task<FileData> CreateTestTrainingDataAsync(OpenAIClient api)
        {
            var fineTuningTrainingData = ScriptableObject.CreateInstance<FineTuningTrainingDataSet>();
            fineTuningTrainingData.ConversationTrainingData = new List<Conversation>
            {
                new Conversation(new List<Message>
                {
                    new Message(Role.System, "Marv is a factual chatbot that is also sarcastic."),
                    new Message(Role.User, "What's the capital of France?"),
                    new Message(Role.Assistant, "Paris, as if everyone doesn't know that already.")
                }),
                new Conversation(new List<Message>
                {
                    new Message(Role.System, "Marv is a factual chatbot that is also sarcastic."),
                    new Message(Role.User, "Who wrote 'Romeo and Juliet'?"),
                    new Message(Role.Assistant, "Oh, just some guy named William Shakespeare. Ever heard of him?")
                }),
                new Conversation(new List<Message>
                {
                    new Message(Role.System, "Marv is a factual chatbot that is also sarcastic."),
                    new Message(Role.User, "How far is the Moon from Earth?"),
                    new Message(Role.Assistant, "Around 384,400 kilometers. Give or take a few, like that really matters.")
                })
            };

            const string localTrainingDataPath = "fineTunesTestTrainingData.jsonl";
            await File.WriteAllLinesAsync(localTrainingDataPath, fineTuningTrainingData.ConversationTrainingToJsonl());

            var fileData = await api.FilesEndpoint.UploadFileAsync(localTrainingDataPath, "fine-tune");
            File.Delete(localTrainingDataPath);
            Assert.IsFalse(File.Exists(localTrainingDataPath));
            return fileData;
        }

        [Test]
        public async Task Test_01_CreateFineTuneJob()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fileData = await CreateTestTrainingDataAsync(api);
            Assert.IsNotNull(fileData);
            var request = new CreateFineTuneJobRequest(Model.GPT3_5_Turbo, fileData);
            var fineTuneResponse = await api.FineTuningEndpoint.CreateFineTuneJobAsync(request);

            Assert.IsNotNull(fineTuneResponse);
            var result = await api.FilesEndpoint.DeleteFileAsync(fileData);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_02_ListFineTuneJobs()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
            Assert.IsNotNull(fineTuneJobs);
            Assert.IsNotEmpty(fineTuneJobs);

            foreach (var job in fineTuneJobs)
            {
                Debug.Log($"{job.Id} -> {job.CreatedAt} | {job.Status}");
            }
        }

        [Test]
        public async Task Test_03_RetrieveFineTuneJobInfo()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
            Assert.IsNotNull(fineTuneJobs);
            Assert.IsNotEmpty(fineTuneJobs);

            foreach (var job in fineTuneJobs)
            {
                var request = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(job);
                Assert.IsNotNull(request);
                Debug.Log($"{request.Id} -> {request.Status}");
            }
        }

        [Test]
        public async Task Test_04_ListFineTuneEvents()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
            Assert.IsNotNull(fineTuneJobs);
            Assert.IsNotEmpty(fineTuneJobs);

            foreach (var job in fineTuneJobs)
            {
                if (job.Status == JobStatus.Cancelled)
                {
                    continue;
                }

                var fineTuneEvents = await api.FineTuningEndpoint.ListFineTuneEventsAsync(job);
                Assert.IsNotNull(fineTuneEvents);
                Assert.IsNotEmpty(fineTuneEvents);

                Debug.Log($"{job.Id} -> status: {job.Status} | event count: {fineTuneEvents.Count} | date: {job.CreatedAt}");

                foreach (var @event in fineTuneEvents)
                {
                    Debug.Log($"  {@event.CreatedAt} [{@event.Level}] {@event.Message.Replace("\n", " ")}");
                }
            }
        }

        [Test]
        public async Task Test_05_CancelFineTuneJob()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
            Assert.IsNotNull(fineTuneJobs);
            Assert.IsNotEmpty(fineTuneJobs);

            foreach (var job in fineTuneJobs)
            {
                if (job.Status == JobStatus.Pending)
                {
                    var result = await api.FineTuningEndpoint.CancelFineTuneJobAsync(job);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result);
                    Debug.Log($"{job.Id} -> cancelled");
                }
            }
        }

        [Test]
        public async Task Test_06_StreamFineTuneEvents()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FineTuningEndpoint);

            var fileData = await CreateTestTrainingDataAsync(api);
            Assert.IsNotNull(fileData);
            var request = new CreateFineTuneJobRequest(Model.GPT3_5_Turbo, fileData);
            var fineTuneResponse = await api.FineTuningEndpoint.CreateFineTuneJobAsync(request);
            Assert.IsNotNull(fineTuneResponse);

            var fineTuneJob = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneResponse);
            Assert.IsNotNull(fineTuneJob);
            Debug.Log($"{fineTuneJob.Id} ->");
            var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await api.FineTuningEndpoint.StreamFineTuneEventsAsync(fineTuneJob, fineTuneEvent =>
                {
                    Debug.Log($"  {fineTuneEvent.CreatedAt} [{fineTuneEvent.Level}] {fineTuneEvent.Message}");
                    cancellationTokenSource.Cancel();
                }, cancelJob: true, cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case TaskCanceledException:
                    case OperationCanceledException:
                        // Ignored
                        break;
                    default:
                        Debug.LogError(e);
                        break;
                }
            }

            var jobInfo = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneJob, CancellationToken.None);
            Assert.IsNotNull(jobInfo);
            Debug.Log($"{jobInfo.Id} -> {jobInfo.Status}");
            Assert.IsTrue(jobInfo.Status == JobStatus.Cancelled);
            var result = await api.FilesEndpoint.DeleteFileAsync(fileData, CancellationToken.None);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_08_DeleteFineTunedModel()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ModelsEndpoint);

            var models = await api.ModelsEndpoint.GetModelsAsync();
            Assert.IsNotNull(models);
            Assert.IsNotEmpty(models);

            try
            {
                foreach (var model in models)
                {
                    if (model.OwnedBy.Contains("openai") ||
                        model.OwnedBy.Contains("system"))
                    {
                        continue;
                    }

                    Console.WriteLine(model);
                    var result = await api.ModelsEndpoint.DeleteFineTuneModelAsync(model);
                    Assert.IsNotNull(result);
                    Assert.IsTrue(result);
                    Console.WriteLine($"{model.Id} -> deleted");
                    break;
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You have insufficient permissions for this operation. You need to be this role: Owner.");
            }
        }
    }
}
