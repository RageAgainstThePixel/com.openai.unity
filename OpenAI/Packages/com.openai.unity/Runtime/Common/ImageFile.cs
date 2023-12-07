// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class ImageFile
    {
        [Preserve]
        [JsonConstructor]
        public ImageFile([JsonProperty("file_id")] string fileId)
        {
            FileId = fileId;
        }

        [Preserve]
        [JsonProperty("file_id")]
        public string FileId { get; private set; }
    }
}
