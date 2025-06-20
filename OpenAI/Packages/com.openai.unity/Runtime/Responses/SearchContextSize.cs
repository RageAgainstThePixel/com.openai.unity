// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum SearchContextSize
    {
        [EnumMember(Value = "low")]
        Low = 1,
        [EnumMember(Value = "medium")]
        Medium,
        [EnumMember(Value = "high")]
        High
    }
}
