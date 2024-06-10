// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.VectorStores
{
    [Preserve]
    public sealed class FileCounts
    {
        [Preserve]
        [JsonConstructor]
        internal FileCounts(
            [JsonProperty("in_progress")] int inProgress,
            [JsonProperty("completed")] int completed,
            [JsonProperty("failed")] int failed,
            [JsonProperty("cancelled")] int cancelled,
            [JsonProperty("total")] int total)
        {
            InProgress = inProgress;
            Completed = completed;
            Failed = failed;
            Cancelled = cancelled;
            Total = total;
        }

        /// <summary>
        /// The number of files that are currently being processed.
        /// </summary>
        [Preserve]
        [JsonProperty("in_progress")]
        public int InProgress { get; }

        /// <summary>
        /// The number of files that have been successfully processed.
        /// </summary>
        [Preserve]
        [JsonProperty("completed")]
        public int Completed { get; }

        /// <summary>
        /// The number of files that have failed to process.
        /// </summary>
        [Preserve]
        [JsonProperty("failed")]
        public int Failed { get; }

        /// <summary>
        /// The number of files that were cancelled.
        /// </summary>
        [Preserve]
        [JsonProperty("cancelled")]
        public int Cancelled { get; }

        /// <summary>
        /// The total number of files.
        /// </summary>
        [Preserve]
        [JsonProperty("total")]
        public int Total { get; }
    }
}
