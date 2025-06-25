// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace OpenAI.Responses
{
    internal class FilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => serializer.Serialize(writer, value);

        public override bool CanConvert(Type objectType) => typeof(IFilter) == objectType;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var type = jObject["type"]!.Value<string>();

            switch (type)
            {
                case "eq":
                case "ne":
                case "gt":
                case "gte":
                case "lt":
                case "lte":
                    return jObject.ToObject<ComparisonFilter>(serializer);
                case "or":
                case "and":
                    return jObject.ToObject<CompoundFilter>(serializer);
                default:
                    throw new NotImplementedException($"Unknown filter type: {type}");
            }
        }
    }
}
