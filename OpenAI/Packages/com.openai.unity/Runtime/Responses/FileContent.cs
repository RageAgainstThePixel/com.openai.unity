// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    [Preserve]
    public sealed class FileContent : BaseResponse, IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal FileContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("file_data")] string fileData,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("file_name")] string fileName,
            [JsonProperty("file_url")] string fileUrl)
        {
            Type = type;
            FileData = fileData;
            FileId = fileId;
            FileName = fileName;
            FileUrl = fileUrl;
        }

        [Preserve]
        public FileContent(string fileName, string fileData)
        {
            Type = ResponseContentType.InputFile;
            FileData = fileData;
            FileName = fileName;
        }

        /// <summary>
        /// The input file, can be a file id or a file url.
        /// </summary>
        /// <param name="fileId">The id or url of the file.</param>
        /// <remarks>If the fileId starts with "http" or "https", it is a file url, otherwise it is a file id.</remarks>
        [Preserve]
        public FileContent(string fileId)
        {
            Type = ResponseContentType.InputFile;

            if (fileId.StartsWith("http"))
            {
                FileUrl = fileId;
            }
            else
            {
                FileId = fileId;
            }
        }

        [Preserve]
        public FileContent(byte[] fileData, string fileName)
        {
            Type = ResponseContentType.InputFile;
            FileData = System.Convert.ToBase64String(fileData);
            FileName = fileName;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("file_data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileData { get; }

        [Preserve]
        [JsonProperty("file_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileId { get; }

        [Preserve]
        [JsonProperty("file_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileUrl { get; private set; }

        [Preserve]
        [JsonProperty("file_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { get; }

        [JsonIgnore]
        public string Object => Type.ToString();
    }
}
