// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public enum RealtimeContentType
    {
        [EnumMember(Value = "output_text")]
        OutputText,
        [EnumMember(Value = "output_audio")]
        OutputAudio,
        [EnumMember(Value = "input_text")]
        InputText,
        [EnumMember(Value = "input_audio")]
        InputAudio,
        [EnumMember(Value = "item_reference")]
        ItemReference
    }
}
