// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class LocalShellAction
    {
        [Preserve]
        [JsonConstructor]
        internal LocalShellAction(
            [JsonProperty("type")] string type,
            [JsonProperty("command")] List<string> command,
            [JsonProperty("env")] Dictionary<string, string> environment,
            [JsonProperty("timeout_ms")] int? timeoutMilliseconds,
            [JsonProperty("user")] string user,
            [JsonProperty("working_directory")] string workingDirectory)
        {
            Type = type;
            Command = command;
            Environment = environment;
            TimeoutMilliseconds = timeoutMilliseconds;
            User = user;
            WorkingDirectory = workingDirectory;
        }

        [Preserve]
        public LocalShellAction(string command,
            IReadOnlyDictionary<string, string> environment = null,
            int? timeoutMilliseconds = null,
            string user = null,
            string workingDirectory = null)
            : this(new[] { command }, environment, timeoutMilliseconds, user, workingDirectory)
        {
        }

        [Preserve]
        public LocalShellAction(
            IEnumerable<string> command,
            IReadOnlyDictionary<string, string> environment = null,
            int? timeoutMilliseconds = null,
            string user = null,
            string workingDirectory = null)
        {
            Type = "exec";
            Command = command.ToList();
            Environment = environment ?? new Dictionary<string, string>();
            TimeoutMilliseconds = timeoutMilliseconds;
            User = user;
            WorkingDirectory = workingDirectory;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; }

        /// <summary>
        /// The command to run.
        /// </summary>
        [Preserve]
        [JsonProperty("command")]
        public IReadOnlyList<string> Command { get; }

        /// <summary>
        /// Environment variables to set for the command.
        /// </summary>
        [Preserve]
        [JsonProperty("env")]
        public IReadOnlyDictionary<string, string> Environment { get; }

        /// <summary>
        /// Optional timeout in milliseconds for the command.
        /// </summary>
        [Preserve]
        [JsonProperty("timeout_ms", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? TimeoutMilliseconds { get; }

        /// <summary>
        /// Optional user to run the command as.
        /// </summary>
        [Preserve]
        [JsonProperty("user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string User { get; }

        /// <summary>
        /// Optional working directory to run the command in.
        /// </summary>
        [Preserve]
        [JsonProperty("working_directory", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string WorkingDirectory { get; }
    }
}
