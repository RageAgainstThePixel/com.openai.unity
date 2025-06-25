// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Responses
{
    internal class ToolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => serializer.Serialize(writer, value);

        public override bool CanConvert(Type objectType) => typeof(ITool) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "code_interpreter" => jObject.ToObject<CodeInterpreterTool>(serializer),
                "computer_use_preview" => jObject.ToObject<ComputerUsePreviewTool>(serializer),
                "file_search" => jObject.ToObject<FileSearchTool>(serializer),
                "function" => jObject.ToObject<Function>(serializer),
                "image_generation" => jObject.ToObject<ImageGenerationTool>(serializer),
                "local_shell" => jObject.ToObject<LocalShellTool>(serializer),
                "mcp" => jObject.ToObject<MCPTool>(serializer),
                "tool" => jObject.ToObject<Tool>(serializer),
                "web_search_preview" => jObject.ToObject<WebSearchPreviewTool>(serializer),
                _ => throw new NotImplementedException($"Unknown tool type: {type}")
            };
        }
    }
}
