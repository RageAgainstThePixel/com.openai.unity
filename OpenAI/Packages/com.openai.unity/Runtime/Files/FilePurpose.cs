// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;

namespace OpenAI.Files
{
    [Preserve]
    public class FilePurpose
    {
        public static readonly FilePurpose Assistants = "assistants";
        public static readonly FilePurpose Batch = "batch";
        public static readonly FilePurpose FineTune = "fine-tune";
        public static readonly FilePurpose Vision = "vision";

        [Preserve]
        public FilePurpose(string purpose) => Value = purpose;

        [Preserve]
        public string Value { get; }

        [Preserve]
        public override string ToString() => Value;

        [Preserve]
        public static implicit operator FilePurpose(string purpose) => new(purpose);

        [Preserve]
        public static implicit operator string(FilePurpose purpose) => purpose?.ToString();
    }
}
