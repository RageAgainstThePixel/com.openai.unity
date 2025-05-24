// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public enum IncompleteReason
    {
        None = 0,
        [EnumMember(Value = "content_filter")]
        ContentFilter,
        [EnumMember(Value = "max_output_tokens")]
        MaxOutputTokens
    }
}
