// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using NUnit.Framework;
using OpenAI.Responses;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_14_Responses : AbstractTestFixture
    {
        [Test]
        public void Test_00_01_ResponseRequest_Serialization()
        {
            const string content = "Tell me a three sentence bedtime story about a unicorn.";
            CreateResponseRequest request = content;
            Assert.NotNull(request);
            Assert.IsNotEmpty(request.Input);
            Assert.IsNotNull(request.Input[0]);
            Assert.IsInstanceOf<MessageItem>(request.Input[0]);
            var messageItem = request.Input[0] as MessageItem;
            Assert.NotNull(messageItem);
            Assert.IsNotEmpty(messageItem!.Content);
            Assert.IsInstanceOf<Responses.TextContent>(messageItem.Content[0]);
            var textContent = messageItem.Content[0] as Responses.TextContent;
            Assert.NotNull(textContent);
            Assert.IsNotEmpty(textContent!.Text);
            Assert.AreEqual(content, textContent.Text);
            var jsonPayload = JsonConvert.SerializeObject(request, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
            Assert.IsNotEmpty(jsonPayload);
            Debug.Log(jsonPayload);
        }

        [Test]
        public async Task Test_01_01_SimpleTextInput()
        {
            Assert.NotNull(OpenAIClient.ResponsesEndpoint);
            var response = await OpenAIClient.ResponsesEndpoint.CreateModelResponseAsync("Tell me a three sentence bedtime story about a unicorn.");
            Assert.NotNull(response);
            Assert.IsNotEmpty(response.Id);
            Assert.AreEqual(ResponseStatus.Completed, response.Status);
        }
    }
}
