// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;

namespace OpenAI.Files
{
    [Preserve]
    public class FilePurpose
    {
        public const string Assistants = "assistants";
        public const string Batch = "batch";
        public const string FineTune = "fine-tune";
        public const string Vision = "vision";

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
