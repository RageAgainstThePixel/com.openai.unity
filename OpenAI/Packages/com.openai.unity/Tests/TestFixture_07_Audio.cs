// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using NUnit.Framework;
using OpenAI.Audio;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_07_Audio
    {
        [Test]
        public async Task Test_1_Transcription_AudioClip()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(audioClip, temperature: 0.1f, language: "en");
            var result = await api.AudioEndpoint.CreateTranscriptionAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_1_Transcription_Path()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            using var request = new AudioTranscriptionRequest(Path.GetFullPath(audioPath), temperature: 0.1f, language: "en");
            var result = await api.AudioEndpoint.CreateTranscriptionAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_2_Translation_AudioClip()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranslationRequest(audioClip);
            var result = await api.AudioEndpoint.CreateTranslationAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_2_Translation_Path()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            using var request = new AudioTranslationRequest(Path.GetFullPath(audioPath));
            var result = await api.AudioEndpoint.CreateTranslationAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_3_Speech()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.AudioEndpoint);
            var request = new SpeechRequest("Hello world!");
            var (path, clip) = await api.AudioEndpoint.CreateSpeechAsync(request);
            Debug.Log(path);
            Assert.IsNotNull(clip);
        }
    }
}
