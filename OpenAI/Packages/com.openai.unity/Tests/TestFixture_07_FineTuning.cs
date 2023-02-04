// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Files;
using OpenAI.FineTuning;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_07_FineTuning
    {
        private async Task<FileData> CreateTestTrainingDataAsync(OpenAIClient api)
        {
            var lines = new List<string>
            {
                new FineTuningTrainingData("Company: BHFF insurance\\nProduct: allround insurance\\nAd:One stop shop for all your insurance needs!\\nSupported:", "yes"),
                new FineTuningTrainingData("Company: Loft conversion specialists\\nProduct: -\\nAd:Straight teeth in weeks!\\nSupported:", "no")
            };

            const string localTrainingDataPath = "fineTunesTestTrainingData.jsonl";
            await File.WriteAllLinesAsync(localTrainingDataPath, lines);

            var fileData = await api.FilesEndpoint.UploadFileAsync(localTrainingDataPath, "fine-tune");
            File.Delete(localTrainingDataPath);
            Assert.IsFalse(File.Exists(localTrainingDataPath));
            return fileData;
        }

        [UnityTest]
        public IEnumerator Test_01_CreateFineTuneJob()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fileData = await CreateTestTrainingDataAsync(api);
                var request = new CreateFineTuneJobRequest(fileData);
                var fineTuneResponse = await api.FineTuningEndpoint.CreateFineTuneJobAsync(request);

                Assert.IsNotNull(fineTuneResponse);
                var result = await api.FilesEndpoint.DeleteFileAsync(fileData);
                Assert.IsTrue(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_02_ListFineTuneJobs()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
                Assert.IsNotNull(fineTuneJobs);
                Assert.IsNotEmpty(fineTuneJobs);

                foreach (var job in fineTuneJobs)
                {
                    Debug.Log($"{job.Id} -> {job.CreatedAt} | {job.Status}");
                }
            });
        }

        [UnityTest]
        public IEnumerator Test_03_RetrieveFineTuneJobInfo()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
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
            });
        }

        [UnityTest]
        public IEnumerator Test_04_ListFineTuneEvents()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
                Assert.IsNotNull(fineTuneJobs);
                Assert.IsNotEmpty(fineTuneJobs);

                foreach (var job in fineTuneJobs)
                {
                    if (job.Status == "cancelled")
                    {
                        continue;
                    }

                    var fineTuneEvents = await api.FineTuningEndpoint.ListFineTuneEventsAsync(job);
                    Assert.IsNotNull(fineTuneEvents);
                    Assert.IsNotEmpty(fineTuneEvents);

                    Debug.Log($"{job.Id} -> status: {job.Status} | event count: {fineTuneEvents.Count}");

                    foreach (var @event in fineTuneEvents)
                    {
                        Debug.Log($"  {@event.CreatedAt} [{@event.Level}] {@event.Message}");
                    }

                    Debug.Log("");
                }
            });
        }

        [UnityTest]
        public IEnumerator Test_05_CancelFineTuneJob()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fineTuneJobs = await api.FineTuningEndpoint.ListFineTuneJobsAsync();
                Assert.IsNotNull(fineTuneJobs);
                Assert.IsNotEmpty(fineTuneJobs);

                foreach (var job in fineTuneJobs)
                {
                    if (job.Status == "pending")
                    {
                        var result = await api.FineTuningEndpoint.CancelFineTuneJobAsync(job);
                        Assert.IsNotNull(result);
                        Assert.IsTrue(result);
                        Debug.Log($"{job.Id} -> cancelled");
                    }
                }
            });
        }

        [UnityTest]
        public IEnumerator Test_06_StreamFineTuneEvents()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fileData = await CreateTestTrainingDataAsync(api);
                var request = new CreateFineTuneJobRequest(fileData);
                var fineTuneResponse = await api.FineTuningEndpoint.CreateFineTuneJobAsync(request);
                Assert.IsNotNull(fineTuneResponse);

                var fineTuneJob = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneResponse);
                Assert.IsNotNull(fineTuneJob);
                Debug.Log($"{fineTuneJob.Id} ->");
                var cancellationTokenSource = new CancellationTokenSource();

                await api.FineTuningEndpoint.StreamFineTuneEventsAsync(fineTuneJob, fineTuneEvent =>
                {
                    Debug.Log($"  {fineTuneEvent.CreatedAt} [{fineTuneEvent.Level}] {fineTuneEvent.Message}");
                    cancellationTokenSource.Cancel();
                }, cancellationTokenSource.Token);

                var jobInfo = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneJob);
                Assert.IsNotNull(jobInfo);
                Debug.Log($"{jobInfo.Id} -> {jobInfo.Status}");
                Assert.IsTrue(jobInfo.Status == "cancelled");
                var result = await api.FilesEndpoint.DeleteFileAsync(fileData, CancellationToken.None);
                Assert.IsTrue(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_07_StreamFineTuneEventsEnumerable()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.FineTuningEndpoint);

                var fileData = await CreateTestTrainingDataAsync(api);
                var request = new CreateFineTuneJobRequest(fileData);
                var fineTuneResponse = await api.FineTuningEndpoint.CreateFineTuneJobAsync(request);
                Assert.IsNotNull(fineTuneResponse);

                var fineTuneJob = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneResponse);
                Assert.IsNotNull(fineTuneJob);
                Debug.Log($"{fineTuneJob.Id} ->");
                var cancellationTokenSource = new CancellationTokenSource();

                await foreach (var fineTuneEvent in api.FineTuningEndpoint.StreamFineTuneEventsEnumerableAsync(
                                   fineTuneJob, cancellationTokenSource.Token))
                {
                    Debug.Log($"  {fineTuneEvent.CreatedAt} [{fineTuneEvent.Level}] {fineTuneEvent.Message}");
                    cancellationTokenSource.Cancel();
                }

                var jobInfo = await api.FineTuningEndpoint.RetrieveFineTuneJobInfoAsync(fineTuneJob);
                Assert.IsNotNull(jobInfo);
                Debug.Log($"{jobInfo.Id} -> {jobInfo.Status}");
                Assert.IsTrue(jobInfo.Status == "cancelled");
                var result = await api.FilesEndpoint.DeleteFileAsync(fileData, CancellationToken.None);
                Assert.IsTrue(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_08_DeleteFineTunedModel()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.ModelsEndpoint);

                var models = await api.ModelsEndpoint.GetModelsAsync();
                Assert.IsNotNull(models);
                Assert.IsNotEmpty(models);

                foreach (var model in models)
                {
                    if (model.OwnedBy == api.OpenAIAuthentication.Organization)
                    {
                        Debug.Log(model);
                        var result = await api.ModelsEndpoint.DeleteFineTuneModelAsync(model);
                        Assert.IsNotNull(result);
                        Assert.IsTrue(result);
                        Debug.Log($"{model.Id} -> deleted");
                    }
                }
            });
        }
    }
}
