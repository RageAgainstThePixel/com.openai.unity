// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;
using OpenAI.Chat;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_08_Files
    {
        [Test]
        public async Task Test_01_UploadFile()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FilesEndpoint);
            var testData = new Conversation(new List<Message> { new Message(Role.Assistant, "I'm a learning language model") });
            await File.WriteAllTextAsync("test.jsonl", testData);
            Assert.IsTrue(File.Exists("test.jsonl"));
            var result = await api.FilesEndpoint.UploadFileAsync("test.jsonl", "fine-tune");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.FileName == "test.jsonl");
            Debug.Log($"{result.Id} -> {result.Object}");
            File.Delete("test.jsonl");
            Assert.IsFalse(File.Exists("test.jsonl"));
        }

        [Test]
        public async Task Test_02_ListFiles()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FilesEndpoint);
            var fileList = await api.FilesEndpoint.ListFilesAsync();

            Assert.IsNotNull(fileList);
            Assert.IsNotEmpty(fileList);

            foreach (var file in fileList)
            {
                var fileInfo = await api.FilesEndpoint.GetFileInfoAsync(file);
                Assert.IsNotNull(fileInfo);
                Debug.Log($"{fileInfo.Id} -> {fileInfo.Object}: {fileInfo.FileName} | {fileInfo.Size} bytes");
            }
        }

        [Test]
        public async Task Test_03_DownloadFile()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FilesEndpoint);
            var fileList = await api.FilesEndpoint.ListFilesAsync();

            Assert.IsNotNull(fileList);
            Assert.IsNotEmpty(fileList);

            var testFileData = fileList[0];
            var result = await api.FilesEndpoint.DownloadFileAsync(testFileData);

            Assert.IsNotNull(result);
            Debug.Log(result);
            Assert.IsTrue(File.Exists(result));

            File.Delete(result);
            Assert.IsFalse(File.Exists(result));
        }

        [Test]
        public async Task Test_04_DeleteFiles()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.FilesEndpoint);
            var fileList = await api.FilesEndpoint.ListFilesAsync();
            Assert.IsNotNull(fileList);
            Assert.IsNotEmpty(fileList);

            foreach (var file in fileList)
            {
                var isDeleted = await api.FilesEndpoint.DeleteFileAsync(file);
                Assert.IsTrue(isDeleted);
                Debug.Log($"{file.Id} -> deleted");
            }

            fileList = await api.FilesEndpoint.ListFilesAsync();
            Assert.IsNotNull(fileList);
            Assert.IsEmpty(fileList);
        }
    }
}
