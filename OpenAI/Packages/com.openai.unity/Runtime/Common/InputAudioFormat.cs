// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public enum InputAudioFormat
    {
        [EnumMember(Value = "wav")]
        Wav = 1,
        [EnumMember(Value = "mp3")]
        Mp3
    }
}
