// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Batch
{
    public sealed class BatchErrors
    {
        [Preserve]
        [JsonProperty("data")]
        public IReadOnlyList<Error> Errors { get; private set; }
    }
}
