// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using OpenAI.Moderations;
using System;
using System.Threading.Tasks;

namespace OpenAI.Tests
{
    internal class TestFixture_10_Moderations
    {
        [Test]
        public async Task Test_1_Moderate()
        {
            var api = new OpenAIClient(OpenAIAuthentication.LoadFromEnv());
            var violationResponse = await api.ModerationsEndpoint.GetModerationAsync("I want to kill them.");
            Assert.IsTrue(violationResponse);

            var response = await api.ModerationsEndpoint.CreateModerationAsync(new ModerationsRequest("I love you"));
            Assert.IsNotNull(response);
            Console.WriteLine(response.Results?[0]?.Scores?.ToString());
        }
    }
}
