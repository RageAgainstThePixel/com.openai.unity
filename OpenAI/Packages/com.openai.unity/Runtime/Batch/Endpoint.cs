// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    [Preserve]
    public sealed class Endpoint
    {
        public const string ChatCompletions = "/v1/chat/completions";
        public const string Embeddings = "/v1/embeddings";
        public const string Completions = "/v1/completions";

        [Preserve]
        public Endpoint(string endpoint) => Value = endpoint;

        [Preserve]
        public string Value { get; }

        [Preserve]
        public override string ToString() => Value;

        [Preserve]
        public static implicit operator string(Endpoint endpoint) => endpoint?.ToString();

        [Preserve]
        public static implicit operator Endpoint(string endpoint) => new(endpoint);
    }
}
