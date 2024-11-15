// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine.Scripting;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    [Preserve]
    public sealed class Error : BaseResponse, IServerSentEvent
    {
        [Preserve]
        [JsonConstructor]
        public Error(
            [JsonProperty("code")] string code,
            [JsonProperty("message")] string message,
            [JsonProperty("param")] string parameter = null,
            [JsonProperty("type")] string type = null,
            [JsonProperty("line")] int? line = null)
        {
            Code = code;
            Message = message;
            Parameter = parameter;
            Type = type;
            Line = line;
        }

        [Preserve]
        internal Error(Exception e)
        {
            Type = e.GetType().Name;
            Message = e.Message;
            Exception = e;
        }

        /// <summary>
        /// An error code identifying the error type.
        /// </summary>
        [Preserve]
        [JsonProperty("code")]
        public string Code { get; }

        /// <summary>
        /// A human-readable message providing more details about the error.
        /// </summary>
        [Preserve]
        [JsonProperty("message")]
        public string Message { get; }

        /// <summary>
        /// The name of the parameter that caused the error, if applicable.
        /// </summary>
        [Preserve]
        [JsonProperty("param")]
        public string Parameter { get; }

        /// <summary>
        /// The type.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// The line number of the input file where the error occurred, if applicable.
        /// </summary>
        [Preserve]
        [JsonProperty("line")]
        public int? Line { get; }

        [Preserve]
        [JsonIgnore]
        public string Object => "error";

        [Preserve]
        [JsonIgnore]
        public Exception Exception { get; }

        [Preserve]
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"[{Code}]");

            if (!string.IsNullOrEmpty(Message))
            {
                builder.Append($" {Message}");
            }

            if (!string.IsNullOrEmpty(Type))
            {
                builder.Append($" Type: {Type}");
            }

            if (!string.IsNullOrEmpty(Parameter))
            {
                builder.Append($" Parameter: {Parameter}");
            }

            if (Line.HasValue)
            {
                builder.Append($" Line: {Line.Value}");
            }

            return builder.ToString();
        }

        [Preserve]
        public static implicit operator Exception(Error error)
            => error.Exception ?? new Exception(error.ToString());
    }
}
