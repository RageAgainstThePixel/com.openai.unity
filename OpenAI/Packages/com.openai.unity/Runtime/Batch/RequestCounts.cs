// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    [Preserve]
    public sealed class RequestCounts
    {
        [Preserve]
        [JsonConstructor]
        internal RequestCounts(
            [JsonProperty("total")] int total,
            [JsonProperty("completed")] int completed,
            [JsonProperty("failed")] int failed)
        {
            Total = total;
            Completed = completed;
            Failed = failed;
        }

        /// <summary>
        /// Total number of requests in the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("total")]
        public int Total { get; }

        /// <summary>
        /// Number of requests that have been completed successfully.
        /// </summary>
        [Preserve]
        [JsonProperty("completed")]
        public int Completed { get; }

        /// <summary>
        /// Number of requests that have failed.
        /// </summary>
        [Preserve]
        [JsonProperty("failed")]
        public int Failed { get; }
    }
}
