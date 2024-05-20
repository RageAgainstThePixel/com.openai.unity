// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    public sealed class RequestCounts
    {
        /// <summary>
        /// Total number of requests in the batch.
        /// </summary>
        [Preserve]
        [JsonProperty("total")]
        public int Total { get; private set; }

        /// <summary>
        /// Number of requests that have been completed successfully.
        /// </summary>
        [Preserve]
        [JsonProperty("completed")]
        public int Completed { get; private set; }

        /// <summary>
        /// Number of requests that have failed.
        /// </summary>
        [Preserve]
        [JsonProperty("failed")]
        public int Failed { get; private set; }
    }
}
