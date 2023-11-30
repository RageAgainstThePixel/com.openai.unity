// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public enum AnnotationType
    {
        [EnumMember(Value = "file_citation")]
        FileCitation,
        [EnumMember(Value = "file_path")]
        FilePath
    }
}
