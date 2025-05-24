// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum ResponseStatus
    {
        None = 0,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "queued")]
        Queued,
        [EnumMember(Value = "incomplete")]
        Incomplete
    }
}
