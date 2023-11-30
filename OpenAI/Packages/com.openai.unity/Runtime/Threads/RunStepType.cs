// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public enum RunStepType
    {
        [EnumMember(Value = "message_creation")]
        MessageCreation,
        [EnumMember(Value = "tool_calls")]
        ToolCalls
    }
}
