// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace OpenAI.Tests
{
    internal abstract class AbstractTestFixture
    {
        protected readonly OpenAIClient OpenAIClient;

        protected AbstractTestFixture()
        {
            var auth = new OpenAIAuthentication().LoadDefaultsReversed();
            var settings = new OpenAISettings();
            OpenAIClient = new OpenAIClient(auth, settings);
            OpenAIClient.EnableDebug = true;
        }
    }
}
