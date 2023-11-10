// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace OpenAI.Chat
{
    public enum ContentType
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "image_url")]
        ImageUrl
    }
}