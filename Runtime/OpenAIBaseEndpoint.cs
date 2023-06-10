// Licensed under the MIT License. See LICENSE in the project root for license information.

using Utilities.WebRequestRest;

namespace OpenAI
{
    public abstract class OpenAIBaseEndpoint : BaseEndPoint<OpenAIClient, OpenAIAuthentication, OpenAISettings>
    {
        protected OpenAIBaseEndpoint(OpenAIClient client) : base(client) { }
    }
}
