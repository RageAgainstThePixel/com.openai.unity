// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenAI.Extensions
{
    /// <summary>
    /// Converts empty strings to null values so they are properly ignored by the serializer.
    /// https://stackoverflow.com/questions/39855694/convert-empty-strings-to-null-with-json-net
    /// </summary>
    internal class EmptyToNullStringContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            => type.GetProperties()
                .Select(propertyInfo =>
                {
                    var jsonProperty = base.CreateProperty(propertyInfo, memberSerialization);
                    jsonProperty.ValueProvider = new EmptyToNullStringValueProvider(propertyInfo);
                    return jsonProperty;
                }).ToList();
    }
}
