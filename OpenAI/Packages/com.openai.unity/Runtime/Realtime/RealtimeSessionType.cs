// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public enum RealtimeSessionType
    {
        [EnumMember(Value = "realtime")]
        Realtime,
        [EnumMember(Value = "transcription")]
        Transcription
    }
}
