// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Responses
{
    internal class AnnotationConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => serializer.Serialize(writer, value);

        public override bool CanConvert(Type objectType) => typeof(IAnnotation) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            return type switch
            {
                "file_citation" => jObject.ToObject<FileCitation>(serializer),
                "file_path" => jObject.ToObject<FilePath>(serializer),
                "url_citation" => jObject.ToObject<UrlCitation>(serializer),
                "container_file_citation" => jObject.ToObject<ContainerFileCitation>(serializer),
                _ => throw new NotImplementedException($"Unknown annotation type: {type}")
            };
        }
    }
}
