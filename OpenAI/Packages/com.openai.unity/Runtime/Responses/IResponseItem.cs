// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI.Responses
{
    [Preserve]
    public interface IResponseItem : IListItem, IServerSentEvent
    {
        /// <summary>
        /// The unique ID of this response item.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The type of response item.
        /// </summary>
        public ResponseItemType Type { get; }

        /// <summary>
        /// The status of the response item.
        /// </summary>
        public ResponseStatus Status { get; }
    }
}
