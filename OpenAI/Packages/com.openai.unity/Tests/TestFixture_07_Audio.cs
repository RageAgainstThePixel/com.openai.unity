// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
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
                await Task.CompletedTask;
            });
        }

        [UnityTest]
        public IEnumerator Test_2_Translation()
        {
            yield return AwaitTestUtilities.Await(async () =>
            {
                var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
                Assert.IsNotNull(api.AudioEndpoint);
                await Task.CompletedTask;
            });
        }
    }
}
