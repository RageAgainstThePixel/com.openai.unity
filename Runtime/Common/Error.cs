// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Error
    {
        [Preserve]
        [JsonConstructor]
        public Error(
            [JsonProperty("code")] string code,
            [JsonProperty("message")] string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// One of server_error or rate_limit_exceeded.
        /// </summary>
        [Preserve]
        [JsonProperty("code")]
        public string Code { get; }

        /// <summary>
        /// A human-readable description of the error.
        /// </summary>
        [Preserve]
        [JsonProperty("message")]
        public string Message { get; }
    }
}
