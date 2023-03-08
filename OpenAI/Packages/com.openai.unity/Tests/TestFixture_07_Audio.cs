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
        public IEnumerator Test_1_Transcription()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var transcriptionAudio = AssetDatabase.GUIDToAssetPath("259eaa73cab84284eac307d3134c3ade");
                var request = new AudioTranscriptionRequest(Path.GetFullPath(transcriptionAudio), language: "en");
                var result = await api.AudioEndpoint.CreateTranscriptionAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }

        [UnityTest]
        public IEnumerator Test_2_Translation()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                var translationAudio = AssetDatabase.GUIDToAssetPath("3ab176222366dc241894506c315c6fa4");
                var request = new AudioTranslationRequest(Path.GetFullPath(translationAudio));
                var result = await api.AudioEndpoint.CreateTranslationAsync(request);
                Assert.IsNotNull(result);
                Debug.Log(result);
            });
        }
    }
}
