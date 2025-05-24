// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace OpenAI.Responses
{
    public enum ResponseItemType
    {
        [EnumMember(Value = "message")]
        Message,
        [EnumMember(Value = "item_reference")]
        ItemReference,
        [EnumMember(Value = "function_call")]
        FunctionCall,
        [EnumMember(Value = "function_call_output")]
        FunctionCallOutput,
        [EnumMember(Value = "file_search_call")]
        FileSearchCall,
        [EnumMember(Value = "computer_call")]
        ComputerCall,
        [EnumMember(Value = "computer_call_output")]
        ComputerCallOutput,
        [EnumMember(Value = "web_search_call")]
        WebSearchCall,
        [EnumMember(Value = "reasoning")]
        Reasoning
    }
}
