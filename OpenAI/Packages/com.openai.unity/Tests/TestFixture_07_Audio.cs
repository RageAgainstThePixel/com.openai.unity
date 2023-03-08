// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Audio;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenAI.Tests
{
    internal class TestFixture_07_Audio
    {
        [UnityTest]
        public IEnumerator Test_1_Transcription_AudioClip()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
                var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
                var request = new AudioTranscriptionRequest(audioClip, language: "en");
                var result = await api.AudioEndpoint.CreateTranscriptionAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_1_Transcription_Path()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var audioPath = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
                var request = new AudioTranscriptionRequest(Path.GetFullPath(audioPath), language: "en");
                var result = await api.AudioEndpoint.CreateTranscriptionAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_Translation_AudioClip()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
                var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
                var request = new AudioTranslationRequest(audioClip);
                var result = await api.AudioEndpoint.CreateTranslationAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_Translation_Path()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var audioPath = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
                var request = new AudioTranslationRequest(Path.GetFullPath(audioPath));
                var result = await api.AudioEndpoint.CreateTranslationAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }
    }
}
