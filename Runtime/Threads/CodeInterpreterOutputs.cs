// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI.Threads
{
    [Preserve]
    public sealed class CodeInterpreterOutputs : IAppendable<CodeInterpreterOutputs>
    {
        [Preserve]
        [JsonConstructor]
        internal CodeInterpreterOutputs(
            [JsonProperty("index")] int? index,
            [JsonProperty("type")] CodeInterpreterOutputType type,
            [JsonProperty("logs")] string logs,
            [JsonProperty("image")] ImageFile image,
            [JsonProperty("files")] List<FilePath> files)
        {
            Index = index;
            Type = type;
            Logs = logs;
            Image = image;
            Files = files;
        }

        [Preserve]
        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Index { get; private set; }

        /// <summary>
        /// Output type. Can be either 'logs' or 'image'.
        /// </summary>
        [Preserve]
        [JsonProperty("type")]
        public CodeInterpreterOutputType Type { get; private set; }

        /// <summary>
        /// Text output from the Code Interpreter tool call as part of a run step.
        /// </summary>
        [Preserve]
        [JsonProperty("logs", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Logs { get; private set; }

        /// <summary>
        /// Code interpreter image output.
        /// </summary>
        [Preserve]
        [JsonProperty("image", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ImageFile Image { get; private set; }

        /// <summary>
        /// Code interpreter file output.
        /// </summary>
        [Preserve]
        [JsonProperty("files", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReadOnlyList<FilePath> Files { get; private set; }

        [Preserve]
        public void AppendFrom(CodeInterpreterOutputs other)
        {
            if (other == null) { return; }

            if (Type == 0 && other.Type > 0)
            {
                Type = other.Type;
            }

            if (other.Index.HasValue)
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other.Logs))
            {
                Logs += other.Logs;
            }

            if (other.Image != null)
            {
                if (Image == null)
                {
                    Image = other.Image;
                }
                else
                {
                    Image.AppendFrom(other.Image);
                }
            }

            if (other.Files is { Count: > 0 })
            {
                Files ??= other.Files;
            }
        }
    }
}
