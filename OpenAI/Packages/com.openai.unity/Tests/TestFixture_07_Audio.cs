// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_07_Audio : AbstractTestFixture
    {
        [Test]
        public async Task Test_01_01_Transcription_Text()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            using var request = new AudioTranscriptionRequest(
                audioPath: Path.GetFullPath(audioPath),
                responseFormat: AudioResponseFormat.Text,
                temperature: 0.1f,
                language: "en");
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionTextAsync(request);
            Assert.IsNotNull(response);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_01_02_01_Transcription_Json()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(
                audio: audioClip,
                model: Model.Transcribe_GPT_4o_Mini,
                responseFormat: AudioResponseFormat.Json,
                temperature: 0.1f,
                language: "en",
                include: new[] { "logprobs" });
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionJsonAsync(request);
            Assert.IsNotNull(response);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_01_02_02_Transcription_Json_ChunkingStrategy_Auto()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(
                audio: audioClip,
                model: Model.Transcribe_GPT_4o_Mini,
                responseFormat: AudioResponseFormat.Json,
                temperature: 0.1f,
                language: "en",
                chunkingStrategy: ChunkingStrategy.Auto);
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionJsonAsync(request);
            Assert.IsNotNull(response);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_01_02_03_Transcription_Json_ChunkingStrategy_Specific()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            var chunkStrategy = new ChunkingStrategy(300, 200, 0.5f);
            using var request = new AudioTranscriptionRequest(
                audio: audioClip,
                model: Model.Transcribe_GPT_4o_Mini,
                responseFormat: AudioResponseFormat.Json,
                temperature: 0.1f,
                language: "en",
                chunkingStrategy: chunkStrategy);
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionJsonAsync(request);
            Assert.IsNotNull(response);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_01_03_01_Transcription_VerboseJson()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(
                audio: audioClip,
                responseFormat: AudioResponseFormat.Verbose_Json,
                temperature: 0.1f,
                language: "en");
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionJsonAsync(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Duration);
            Assert.IsTrue(response.Language == "english");
            Assert.IsNotNull(response.Segments);
            Assert.IsNotEmpty(response.Segments);
        }

        [Test]
        public async Task Test_01_03_02_Transcription_VerboseJson_WordSimilarities()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranscriptionRequest(
                audio: audioClip,
                responseFormat: AudioResponseFormat.Verbose_Json,
                timestampGranularity: TimestampGranularity.Word,
                temperature: 0.1f,
                language: "en");
            var response = await OpenAIClient.AudioEndpoint.CreateTranscriptionJsonAsync(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Duration);
            Assert.IsTrue(response.Language == "english");
            Assert.IsNotNull(response.Words);
            Assert.IsNotEmpty(response.Words);
        }

        [Test]
        public async Task Test_02_01_Translation_Path()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            using var request = new AudioTranslationRequest(Path.GetFullPath(audioPath));
            var result = await OpenAIClient.AudioEndpoint.CreateTranslationTextAsync(request);
            Assert.IsNotNull(result);
            Debug.Log(result);
        }

        [Test]
        public async Task Test_02_02_Translation_AudioClip()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
            var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            using var request = new AudioTranslationRequest(
                audio: audioClip,
                prompt: "responses should be in spanish.");
            var response = await OpenAIClient.AudioEndpoint.CreateTranslationJsonAsync(request);
            Assert.IsNotNull(response);
            Debug.Log(response);
        }

        [Test]
        public async Task Test_03_01_01_Speech()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var request = new SpeechRequest("Hello world!");
            var speechClip = await OpenAIClient.AudioEndpoint.GetSpeechAsync(request);
            Debug.Log(speechClip.CachePath);
            Assert.IsNotEmpty(speechClip.AudioSamples);
            Assert.IsNotNull(speechClip.AudioClip);
        }

        [Test]
        public async Task Test_03_01_02_SpeechWithInstructions()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            const string instructions = "You are a computer program giving your first response to eager computer scientists. Slightly digitize your voice and make it sounds like the matrix.";
            var request = new SpeechRequest(
                input: "Hello world!",
                model: Model.TTS_GPT_4o_Mini,
                voice: Voice.Fable,
                instructions: instructions);
            var speechClip = await OpenAIClient.AudioEndpoint.GetSpeechAsync(request);
            Debug.Log(speechClip.CachePath);
            Assert.IsNotEmpty(speechClip.AudioSamples);
            Assert.IsNotNull(speechClip.AudioClip);
        }

        [Test]
        public async Task Test_03_02_01_Speech_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            var request = new SpeechRequest(
                input: "Hello world!",
                responseFormat: SpeechResponseFormat.PCM);
            var clipQueue = new ConcurrentQueue<SpeechClip>();
            var speechClip = await OpenAIClient.AudioEndpoint.GetSpeechAsync(request, partialClip => clipQueue.Enqueue(partialClip));
            Debug.Log(speechClip.CachePath);
            Assert.IsNotEmpty(speechClip.AudioSamples);
            Assert.IsNotNull(speechClip.AudioClip);
            Assert.IsFalse(clipQueue.IsEmpty);
        }

        [Test]
        public async Task Test_03_02_02_SpeechWithInstructions_Streaming()
        {
            Assert.IsNotNull(OpenAIClient.AudioEndpoint);
            const string instructions = "You are a computer program giving your first response to eager computer scientists. Slightly digitize your voice and make it sounds like the matrix.";
            var request = new SpeechRequest(
                input: "Hello world!",
                model: Model.TTS_GPT_4o_Mini,
                voice: Voice.Fable,
                responseFormat: SpeechResponseFormat.PCM,
                instructions: instructions);
            var clipQueue = new ConcurrentQueue<SpeechClip>();
            var speechClip = await OpenAIClient.AudioEndpoint.GetSpeechAsync(request, partialClip => clipQueue.Enqueue(partialClip));
            Debug.Log(speechClip.CachePath);
            Assert.IsNotEmpty(speechClip.AudioSamples);
            Assert.IsNotNull(speechClip.AudioClip);
            Assert.IsFalse(clipQueue.IsEmpty);
        }
    }
}
