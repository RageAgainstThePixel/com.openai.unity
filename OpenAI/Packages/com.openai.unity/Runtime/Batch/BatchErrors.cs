// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    [Preserve]
    public sealed class BatchErrors
    {
        [Preserve]
        [JsonConstructor]
        internal BatchErrors([JsonProperty("data")] List<Error> errors)
        {
            Errors = errors;
        }

        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<Error> Errors { get; }
    }
}
