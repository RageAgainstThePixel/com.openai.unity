// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum ComputerClickButtonType
    {
        [EnumMember(Value = "left")]
        Left = 1,
        [EnumMember(Value = "right")]
        Right,
        [EnumMember(Value = "wheel")]
        Wheel,
        [EnumMember(Value = "back")]
        Back,
        [EnumMember(Value = "forward")]
        Forward
    }
}
