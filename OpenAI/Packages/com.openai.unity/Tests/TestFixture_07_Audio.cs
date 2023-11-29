// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Audio;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_07_Audio : AbstractTestFixture
    {
        [Test]
        public async Task Test_01_01_Transcription_Path()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            using var request = new AudioTranscriptionRequest(Path.GetFullPath(audioPath), temperature: 0.1f, language: "en");
            var result = await OpenAIClient.AudioEndpoint.CreateTranscriptionAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_01_02_Transcription_AudioClip()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(audioClip, temperature: 0.1f, language: "en");
            var result = await OpenAIClient.AudioEndpoint.CreateTranscriptionAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_02_01_Translation_Path()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            using var request = new AudioTranslationRequest(Path.GetFullPath(audioPath));
            var result = await OpenAIClient.AudioEndpoint.CreateTranslationAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_02_02_Translation_AudioClip()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranslationRequest(audioClip);
            var result = await OpenAIClient.AudioEndpoint.CreateTranslationAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_3_Speech()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var request = new SpeechRequest("Hello world!");
            var (path, clip) = await OpenAIClient.AudioEndpoint.CreateSpeechAsync(request);
            Debug.Log(path);
            Assert.IsNotNull(clip);
        }
    }
}
