// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public enum RunStatus
    {
        [EnumMember(Value = "queued")]
        Queued,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "requires_action")]
        RequiresAction,
        [EnumMember(Value = "cancelling")]
        Cancelling,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "incomplete")]
        Incomplete,
        [EnumMember(Value = "expired")]
        Expired
    }
}
