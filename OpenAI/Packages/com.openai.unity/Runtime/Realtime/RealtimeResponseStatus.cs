// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public enum RealtimeResponseStatus
    {
        [EnumMember(Value = "in_progress")]
        InProgress = 1,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "incomplete")]
        Incomplete
    }
}
