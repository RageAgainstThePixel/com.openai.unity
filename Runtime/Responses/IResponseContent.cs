// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    [Preserve]
    public interface IResponseContent : IServerSentEvent
    {
        public ResponseContentType Type { get; }
    }
}
