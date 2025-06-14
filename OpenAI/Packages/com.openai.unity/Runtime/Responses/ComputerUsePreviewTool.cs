// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Responses
{
    /// <summary>
    /// A tool that controls a virtual computer. Learn more about the computer tool.
    /// </summary>
    [Preserve]
    public sealed class ComputerUsePreviewTool : ITool
    {
        [Preserve]
        public static implicit operator Tool(ComputerUsePreviewTool computerUsePreviewTool) => new(computerUsePreviewTool as ITool);

        [Preserve]
        public ComputerUsePreviewTool(int displayHeight, int displayWidth, string environment)
        {
            DisplayHeight = displayHeight;
            DisplayWidth = displayWidth;
            Environment = environment;
        }

        [Preserve]
        [JsonProperty("type")]
        public string Type => "computer_use_preview";

        /// <summary>
        /// The height of the computer display.
        /// </summary>
        [Preserve]
        [JsonProperty("display_height")]
        public int DisplayHeight { get; private set; }

        /// <summary>
        /// The width of the computer display.
        /// </summary>
        [Preserve]
        [JsonProperty("display_width")]
        public int DisplayWidth { get; private set; }

        /// <summary>
        /// The type of computer environment to control.
        /// </summary>
        [Preserve]
        [JsonProperty("environment")]
        public string Environment { get; private set; }
    }
}
