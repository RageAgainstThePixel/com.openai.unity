// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public enum NoiseReduction
    {
        [EnumMember(Value = "near_field")]
        NearField,
        [EnumMember(Value = "far_field")]
        FarField,
    }
}
