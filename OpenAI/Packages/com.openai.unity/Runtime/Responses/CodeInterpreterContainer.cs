// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class CodeInterpreterContainer
    {
        [Preserve]
        public CodeInterpreterContainer(IEnumerable<string> fileIds)
        {
            FileIds = fileIds?.ToList() ?? throw new NullReferenceException(nameof(fileIds));
        }

        [Preserve]
        [JsonConstructor]
        internal CodeInterpreterContainer(string type, IReadOnlyList<string> fileIds)
        {
            Type = type;
            FileIds = fileIds;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; } = "auto";

        [Preserve]
        [JsonProperty("file_ids")]
        public IReadOnlyList<string> FileIds { get; }
    }
}
