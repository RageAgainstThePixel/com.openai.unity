// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class CompoundFilter : IFilter
    {
        [Preserve]
        public CompoundFilter(ComparisonFilter filter, CompoundFilterOperator type)
            : this(new[] { filter ?? throw new ArgumentNullException(nameof(filter)) }, type)
        {
        }

        [Preserve]
        [JsonConstructor]
        public CompoundFilter(IEnumerable<ComparisonFilter> filters, CompoundFilterOperator type)
        {
            Filters = filters?.ToList() ?? throw new ArgumentNullException(nameof(filters));
            Type = type;
        }

        [Preserve]
        [JsonProperty("type")]
        public CompoundFilterOperator Type { get; }

        [Preserve]
        [JsonProperty("filters")]
        public IReadOnlyList<ComparisonFilter> Filters { get; }
    }
}
