// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace OpenAI.Realtime
{
    public enum RealtimeAudioFormat
    {
        [EnumMember(Value = "audio/pcm")]
        Pcm,
        [EnumMember(Value = "audio/pcmu")]
        G711Ulaw,
        [EnumMember(Value = "audio/pcma")]
        G711Alaw,
    }
}
