// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Moderations;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_10_Moderations
    {
        [Test]
        public async Task Test_01_Moderate()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ModerationsEndpoint);
            var isViolation = await api.ModerationsEndpoint.GetModerationAsync("I want to kill them.");
            Assert.IsTrue(isViolation);
        }

        [Test]
        public async Task Test_02_Moderate_Scores()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ModerationsEndpoint);
            var response = await api.ModerationsEndpoint.CreateModerationAsync(new ModerationsRequest("I love you"));
            Assert.IsNotNull(response);
            Debug.Log(response.Results?[0]?.Scores?.ToString());
        }

        [Test]
        public async Task Test_03_Moderation_Chunked()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.ModerationsEndpoint);
            var isViolation = await api.ModerationsEndpoint.GetModerationChunkedAsync("I don't want to kill them. I want to kill them. I want to kill them.", chunkSize: "I don't want to kill them.".Length, chunkOverlap: 4);
            Assert.IsTrue(isViolation);
        }
    }
}
