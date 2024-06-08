// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
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
            [JsonProperty("image")] ImageFile image)
        {
            Index = index;
            Type = type;
            Logs = logs;
            Image = image;
        }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; }

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
        [JsonProperty("logs")]
        public string Logs { get; private set; }

        /// <summary>
        /// Code interpreter image output.
        /// </summary>
        [Preserve]
        [JsonProperty("image")]
        public ImageFile Image { get; private set; }

        public void Append(CodeInterpreterOutputs other)
        {
            if (other == null) { return; }

            if (Type == 0 && other.Type > 0)
            {
                Type = other.Type;
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
                    Image.Append(other.Image);
                }
            }
        }
    }
}
