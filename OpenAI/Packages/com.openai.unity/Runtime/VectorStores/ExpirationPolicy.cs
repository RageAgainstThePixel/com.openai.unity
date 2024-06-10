// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    /// <summary>
    /// The expiration policy for a vector store.
    /// </summary>
    [Preserve]
    public sealed class ExpirationPolicy
    {
        [Preserve]
        [JsonConstructor]
        internal ExpirationPolicy(
            [JsonProperty("anchor")] string anchor,
            [JsonProperty("days")] int days)
        {
            Anchor = anchor;
            Days = days;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="days">
        /// The number of days after the anchor time that the vector store will expire.
        /// </param>
        public ExpirationPolicy(int days)
        {
            Days = days;
        }

        /// <summary>
        /// Anchor timestamp after which the expiration policy applies.
        /// Supported anchors: 'last_active_at'.
        /// </summary>
        [Preserve]
        [JsonProperty("anchor")]
        public string Anchor { get; } = "last_active_at";

        /// <summary>
        /// The number of days after the anchor time that the vector store will expire.
        /// </summary>
        [Preserve]
        [JsonProperty("days")]
        public int Days { get; }

        [Preserve]
        public static implicit operator ExpirationPolicy(int days) => new(days);
    }
}
