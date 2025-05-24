// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    public interface IResponseItem
    {
        public string Id { get; }

        public ResponseItemType Type { get; }

        public string Object { get; }

        public ResponseStatus Status { get; }
    }

    [Preserve]
    public sealed class MessageItem : IResponseItem
    {
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        [Preserve]
        [JsonProperty("role", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Role Role { get; }

        [Preserve]
        [JsonProperty("content", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<IResponseContent> Content { get; }
    }

    [Preserve]
    public enum ResponseContentType
    {
        [EnumMember(Value = "input_text")]
        InputText,

        [EnumMember(Value = "output_text")]
        OutputText,

        [EnumMember(Value = "input_image")]
        InputImage,

        [EnumMember(Value = "input_file")]
        InputFile,

        [EnumMember(Value = "refusal")]
        Refusal,
    }

    public interface IResponseContent
    {
        public ResponseContentType Type { get; }
    }

    [Preserve]
    public sealed class TextContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal TextContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("text")] string text,
            [JsonProperty("annotations")] IReadOnlyList<Annotation> annotations)
        {
            Type = type;
            Text = text;
            Annotations = annotations ?? new List<Annotation>();
        }

        [Preserve]
        public TextContent(string text)
        {
            Type = ResponseContentType.InputText;
            Text = text;
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("text", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; }

        [Preserve]
        [JsonProperty("annotations", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<Annotation> Annotations { get; }

        [Preserve]
        public static implicit operator TextContent(string input) => new(input);
    }

    [Preserve]
    public sealed class FileContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal FileContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("file_data")] string fileData,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("file_name")] string fileName)
        {
            Type = type;
            FileData = fileData;
            FileId = fileId;
            FileName = fileName;
        }

        [Preserve]
        public FileContent(string fileName, string fileData)
        {
            Type = ResponseContentType.InputFile;
            FileData = fileData;
            FileName = fileName;
        }

        [Preserve]
        public FileContent(string fileId)
        {
            Type = ResponseContentType.InputFile;
            FileId = fileId;
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
        [JsonProperty("file_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileName { get; }
    }

    [Preserve]
    public sealed class ImageContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal ImageContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("detail")] ImageDetail detail,
            [JsonProperty("file_id")] string fileId,
            [JsonProperty("image_url")] string imageUrl)
        {
            Type = type;
            Detail = detail;
            FileId = fileId;
            ImageUrl = imageUrl;
        }

        [Preserve]
        public ImageContent(string imageUrl = null, string fileId = null, ImageDetail detail = ImageDetail.Auto)
        {
            Type = ResponseContentType.InputImage;
            ImageUrl = imageUrl;
            FileId = fileId;
            Detail = detail;
        }

        [Preserve]
        public ImageContent(Texture2D image, ImageDetail detail = ImageDetail.Auto)
        {
            Type = ResponseContentType.InputImage;
            Detail = detail;
            ImageUrl = $"data:image/png;base64,{System.Convert.ToBase64String(image.EncodeToPNG())}";
        }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("detail", DefaultValueHandling = DefaultValueHandling.Include)]
        public ImageDetail Detail { get; }

        [Preserve]
        [JsonProperty("file_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FileId { get; }

        [Preserve]
        [JsonProperty("image_url")]
        public string ImageUrl { get; }

        [Preserve]
        public static implicit operator ImageContent(Texture2D image) => new(image);

        [Preserve]
        public static implicit operator ImageContent(string imageUrl) => new(imageUrl);
    }

    [Preserve]
    public sealed class RefusalContent : IResponseContent
    {
        [Preserve]
        [JsonConstructor]
        internal RefusalContent(
            [JsonProperty("type")] ResponseContentType type,
            [JsonProperty("reason")] string reason)
        {
            Type = type;
            Reason = reason;
        }

        [Preserve]
        [JsonProperty("type")]
        public ResponseContentType Type { get; }

        [Preserve]
        [JsonProperty("reason")]
        public string Reason { get; }
    }

    [Preserve]
    public sealed class FileSearchToolCall : IResponseItem
    {
        [Preserve]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id { get; }

        [Preserve]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Include)]
        public ResponseItemType Type { get; }

        [Preserve]
        [JsonProperty("object", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Object { get; }

        [Preserve]
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseStatus Status { get; }

        [Preserve]
        [JsonProperty("queries")]
        public IReadOnlyList<string> Queries { get; }
    }
}
