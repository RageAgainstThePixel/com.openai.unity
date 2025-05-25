// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum ResponseContentType
    {
        [EnumMember(Value = "input_text")]
        InputText,
        [EnumMember(Value = "output_text")]
        OutputText,
        [EnumMember(Value = "input_audio")]
        InputAudio,
        [EnumMember(Value = "input_image")]
        InputImage,
        [EnumMember(Value = "input_file")]
        InputFile,
        [EnumMember(Value = "refusal")]
        Refusal,
    }
}
