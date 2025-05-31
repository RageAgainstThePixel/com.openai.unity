// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum Truncation
    {
        [EnumMember(Value = "auto")]
        Auto = 1,
        [EnumMember(Value = "disabled")]
        Disabled,
    }
}
