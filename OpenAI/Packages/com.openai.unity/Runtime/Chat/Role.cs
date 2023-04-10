// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace OpenAI.Chat
{
    public enum Role
    {
        [EnumMember(Value = "system")]
        System = 1,
        [EnumMember(Value = "assistant")]
        Assistant = 2,
        [EnumMember(Value = "user")]
        User = 3,
    }
}
