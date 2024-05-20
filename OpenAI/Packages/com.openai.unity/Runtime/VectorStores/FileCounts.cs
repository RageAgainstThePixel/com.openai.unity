// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    public sealed class FileCounts
    {
        /// <summary>
        /// The number of files that are currently being processed.
        /// </summary>
        [Preserve]
        [JsonProperty("in_progress")]
        public int InProgress { get; private set; }

        /// <summary>
        /// The number of files that have been successfully processed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed")]
        public int Completed { get; private set; }

        /// <summary>
        /// The number of files that have failed to process.
        /// </summary>
        [Preserve]
        [JsonProperty("failed")]
        public int Failed { get; private set; }

        /// <summary>
        /// The number of files that were cancelled.
        /// </summary>
        [Preserve]
        [JsonProperty("cancelled")]
        public int Cancelled { get; private set; }

        /// <summary>
        /// The total number of files.
        /// </summary>
        [Preserve]
        [JsonProperty("total")]
        public int Total { get; private set; }
    }
}
