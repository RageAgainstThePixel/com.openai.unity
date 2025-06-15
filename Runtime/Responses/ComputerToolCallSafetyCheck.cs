// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A pending safety check for the computer call.
    /// </summary>
    [Preserve]
    public sealed class ComputerToolCallSafetyCheck
    {
        [Preserve]
        [JsonConstructor]
        internal ComputerToolCallSafetyCheck(
            [JsonProperty("id")] string id,
            [JsonProperty("code")] string code,
            [JsonProperty("message")] string message)
        {
            Id = id;
            Code = code;
            Message = message;
        }

        /// <summary>
        /// The ID of the pending safety check.
        /// </summary>
        [Preserve]
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// The type of the pending safety check.
        /// </summary>
        [Preserve]
        [JsonProperty("code")]
        public string Code { get; }

        /// <summary>
        /// Details about the pending safety check.
        /// </summary>
        [Preserve]
        [JsonProperty("message", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; }
    }
}
