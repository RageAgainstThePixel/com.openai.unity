// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using NUnit.Framework;
using OpenAI.Edits;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    [Obsolete]
    internal class TestFixture_04_Edits
    {
        public async Task Test_1_GetBasicEdit()
        {
            var api = new OpenAIClient(OpenAIAuthentication.Default.LoadFromEnvironment());
            Assert.IsNotNull(api.EditsEndpoint);
            var request = new EditRequest("What day of the wek is it?", "Fix the spelling mistakes");
            var result = await api.EditsEndpoint.CreateEditAsync(request);
            Assert.IsNotNull(result);
            Assert.NotNull(result.Choices);
            Assert.NotZero(result.Choices.Count);
            Debug.Log(result);
            Assert.IsTrue(result.ToString().Contains("week"));
        }
    }
}
