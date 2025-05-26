// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Responses
{
    internal class ResponseContentConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();

        public override bool CanConvert(Type objectType) => typeof(IResponseContent) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "input_text" => jObject.ToObject<TextContent>(serializer),
                "output_text" => jObject.ToObject<TextContent>(serializer),
                "input_audio" => jObject.ToObject<AudioContent>(serializer),
                "input_image" => jObject.ToObject<ImageContent>(serializer),
                "input_file" => jObject.ToObject<FileContent>(serializer),
                "refusal" => jObject.ToObject<RefusalContent>(serializer),
                _ => throw new NotImplementedException($"Unknown response content type: {type}")
            };
        }
    }
}
