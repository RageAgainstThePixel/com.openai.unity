using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public enum VAD_Eagerness
    {
        [EnumMember(Value = "auto")]
        Auto = 0,
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "medium")]
        Medium,
        [EnumMember(Value = "high")]
        High
    }
}