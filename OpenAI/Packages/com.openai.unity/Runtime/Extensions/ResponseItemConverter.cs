// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Responses
{
    internal class ResponseItemConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => serializer.Serialize(writer, value);

        public override bool CanConvert(Type objectType) => typeof(IResponseItem) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "message" => jObject.ToObject<Message>(serializer),
                "computer_call" => jObject.ToObject<ComputerToolCall>(serializer),
                "computer_call_output" => jObject.ToObject<ComputerToolCall>(serializer),
                "function_call" => jObject.ToObject<FunctionToolCall>(serializer),
                "function_call_output" => jObject.ToObject<FunctionToolCallOutput>(serializer),
                "custom_tool_call" => jObject.ToObject<CustomToolCall>(serializer),
                "custom_tool_call_output" => jObject.ToObject<FunctionToolCallOutput>(serializer),
                "image_generation_call" => jObject.ToObject<ImageGenerationCall>(serializer),
                "local_shell_call" => jObject.ToObject<LocalShellCall>(serializer),
                "local_shell_call_output" => jObject.ToObject<LocalShellCall>(serializer),
                "file_search_call" => jObject.ToObject<FileSearchToolCall>(serializer),
                "web_search_call" => jObject.ToObject<WebSearchToolCall>(serializer),
                "reasoning" => jObject.ToObject<ReasoningItem>(serializer),
                "mcp_call" => jObject.ToObject<MCPToolCall>(serializer),
                "mcp_approval_request" => jObject.ToObject<MCPApprovalRequest>(serializer),
                "mcp_approval_response" => jObject.ToObject<MCPApprovalResponse>(serializer),
                "mcp_list_tools" => jObject.ToObject<MCPListTools>(serializer),
                "item_reference" => jObject.ToObject<ItemReference>(serializer),
                _ => throw new NotImplementedException($"Unknown response item type: {type}")
            };
        }
    }
}
