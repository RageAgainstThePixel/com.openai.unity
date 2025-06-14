// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public interface IComputerAction
    {
        public ComputerActionType Type { get; }
    }
}
