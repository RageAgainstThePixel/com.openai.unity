// Licensed under the MIT License. See LICENSE in the project root for license information.

using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    public interface IResponseContent : IServerSentEvent
    {
        public ResponseContentType Type { get; }
    }
}
