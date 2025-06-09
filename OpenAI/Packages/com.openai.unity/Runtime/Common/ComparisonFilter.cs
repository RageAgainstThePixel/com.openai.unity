// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ComparisonFilter : IFilter
    {
        [Preserve]
        [JsonConstructor]
        public ComparisonFilter(string key, ComparisonFilterType type)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Type = type;
        }

        [Preserve]
        [JsonProperty("key")]
        public string Key { get; }

        [Preserve]
        [JsonProperty("type")]
        public ComparisonFilterType Type { get; }
    }
}
