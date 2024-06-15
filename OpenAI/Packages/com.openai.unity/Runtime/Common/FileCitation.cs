// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class FileCitation
    {
        [Preserve]
        [JsonConstructor]
        public FileCitation(
            [JsonProperty("file_id")] string fileId)
        {
            FileId = fileId;
        }

        /// <summary>
        /// The ID of the specific File the citation is from.
        /// </summary>
        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; }

        /// <summary>
        /// The specific quote in the file.
        /// </summary>
        [Obsolete("Removed")]
        [JsonProperty("quote")]
        public string Quote { get; }
    }
}
