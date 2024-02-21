// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenAI.Extensions
{
    internal static class TypeExtensions
    {
        public static JObject GenerateJsonSchema(this MethodInfo methodInfo)
        {
            var schema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject()
            };
            var requiredParameters = new JArray();

            foreach (var parameter in methodInfo.GetParameters())
            {
                if (string.IsNullOrWhiteSpace(parameter.Name))
                {
                    throw new InvalidOperationException($"Failed to find a valid parameter name for {methodInfo.DeclaringType}.{methodInfo.Name}()");
                }

                if (!parameter.HasDefaultValue)
                {
                    requiredParameters.Add(parameter.Name);
                }

                schema["properties"]![parameter.Name] = GenerateJsonSchema(parameter.ParameterType);
            }

            if (requiredParameters.Count > 0)
            {
                schema["required"] = requiredParameters;
            }

            return schema;
        }

        public static JObject GenerateJsonSchema(this Type type)
        {
            var schema = new JObject();

            if (type.IsEnum)
            {
                schema["type"] = "string";
                schema["enum"] = new JArray();

                foreach (var value in Enum.GetValues(type))
                {
                    ((JArray)schema["enum"]).Add(value.ToString());
                }
            }
            else if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                schema["type"] = "array";
                schema["items"] = GenerateJsonSchema(type.GetElementType() ?? type.GetGenericArguments()[0]);
            }
            else if (type.IsClass && type != typeof(string))
            {
                schema["type"] = "object";
                var properties = type.GetProperties();
                var propertiesInfo = new JObject();
                var requiredProperties = new JArray();

                foreach (var property in properties)
                {
                    var propertyInfo = GenerateJsonSchema(property.PropertyType);

                    if (Nullable.GetUnderlyingType(property.PropertyType) == null)
                    {
                        requiredProperties.Add(property.Name);
                    }

                    propertiesInfo[property.Name] = propertyInfo;
                }

                schema["properties"] = propertiesInfo;

                if (requiredProperties.Count > 0)
                {
                    schema["required"] = requiredProperties;
                }
            }
            else
            {
                schema["type"] = type.Name.ToLower();
            }

            return schema;
        }
    }
}
