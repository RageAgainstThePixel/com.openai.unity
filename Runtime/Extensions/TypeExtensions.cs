// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace OpenAI.Extensions
{
    internal static class TypeExtensions
    {
        public static JObject GenerateJsonSchema(this MethodInfo methodInfo, JsonSerializer serializer = null)
        {
            var schema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject()
            };

            var requiredParameters = new JArray();
            var parameters = methodInfo.GetParameters();

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType == typeof(CancellationToken)) { continue; }

                if (string.IsNullOrWhiteSpace(parameter.Name))
                {
                    throw new InvalidOperationException($"Failed to find a valid parameter name for {methodInfo.DeclaringType}.{methodInfo.Name}()");
                }

                if (!parameter.HasDefaultValue)
                {
                    requiredParameters.Add(parameter.Name);
                }

                schema["properties"]![parameter.Name] = GenerateJsonSchema(parameter.ParameterType, schema, serializer);

                var functionParameterAttribute = parameter.GetCustomAttribute<FunctionParameterAttribute>();

                if (functionParameterAttribute != null)
                {
                    schema["properties"]![parameter.Name]!["description"] = functionParameterAttribute.Description;
                }
            }

            if (requiredParameters.Count > 0)
            {
                schema["required"] = requiredParameters;
            }

            schema["additionalProperties"] = false;
            return schema;
        }

        public static JObject GenerateJsonSchema(this Type type, JsonSerializer serializer = null)
        {
            var schema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject()
            };

            var requiredProperties = new JArray();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                var propertyNameAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                var propertyName = propertyNameAttribute?.PropertyName ?? property.Name;
                requiredProperties.Add(propertyName);
                schema["properties"]![propertyName] = GenerateJsonSchema(property.PropertyType, schema, serializer);
            }

            if (requiredProperties.Count > 0)
            {
                schema["required"] = requiredProperties;
            }

            schema["additionalProperties"] = false;
            return schema;
        }

        public static JObject GenerateJsonSchema(this Type type, JObject rootSchema, JsonSerializer serializer = null)
        {
            serializer ??= OpenAIClient.JsonSerializer;
            var schema = new JObject();
            type = UnwrapNullableType(type);

            if (!type.IsPrimitive &&
                type != typeof(Guid) &&
                type != typeof(DateTime) &&
                type != typeof(DateTimeOffset) &&
                rootSchema["definitions"] != null &&
                ((JObject)rootSchema["definitions"]).ContainsKey(type.FullName!))
            {
                return new JObject { ["$ref"] = $"#/definitions/{type.FullName}" };
            }

            if (type.TryGetSimpleTypeSchema(out var schemaType))
            {
                schema["type"] = schemaType;

                if (type == typeof(DateTime) ||
                    type == typeof(DateTimeOffset))
                {
                    schema["format"] = "date-time";
                }
                else if (type == typeof(Guid))
                {
                    schema["format"] = "uuid";
                }
            }
            else if (type.IsEnum)
            {
                schema["type"] = "string";
                schema["enum"] = new JArray(Enum.GetNames(type).Select(JValue.CreateString).ToArray<object>());
            }
            else if (type.TryGetDictionaryValueType(out var valueType))
            {
                schema["type"] = "object";

                if (rootSchema["definitions"] != null &&
                    ((JObject)rootSchema["definitions"]).ContainsKey(valueType.FullName!))
                {
                    schema["additionalProperties"] = new JObject { ["$ref"] = $"#/definitions/{valueType.FullName}" };
                }
                else
                {
                    schema["additionalProperties"] = GenerateJsonSchema(valueType, rootSchema);
                }
            }
            else if (type.TryGetCollectionElementType(out var elementType))
            {
                schema["type"] = "array";

                if (rootSchema["definitions"] != null &&
                    ((JObject)rootSchema["definitions"]).ContainsKey(elementType.FullName!))
                {
                    schema["items"] = new JObject { ["$ref"] = $"#/definitions/{elementType.FullName}" };
                }
                else
                {
                    schema["items"] = GenerateJsonSchema(elementType, rootSchema);
                }
            }
            else
            {
                schema["type"] = "object";
                rootSchema["definitions"] ??= new JObject();
                rootSchema["definitions"][type.FullName!] = new JObject();

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var members = new List<MemberInfo>(properties.Length + fields.Length);
                members.AddRange(properties);
                members.AddRange(fields);

                var memberInfo = new JObject();
                var requiredMembers = new JArray();

                foreach (var member in members)
                {
                    var memberType = GetMemberType(member);
                    var functionPropertyAttribute = member.GetCustomAttribute<FunctionPropertyAttribute>();
                    var jsonPropertyAttribute = member.GetCustomAttribute<JsonPropertyAttribute>();
                    var propertyName = jsonPropertyAttribute?.PropertyName ?? member.Name;

                    // skip unity engine property for Items
                    if (memberType == typeof(float) && propertyName.Equals("Item")) { continue; }

                    JObject propertyInfo;

                    if (rootSchema["definitions"] != null &&
                            ((JObject)rootSchema["definitions"]).ContainsKey(memberType.FullName!))
                    {
                        propertyInfo = new JObject { ["$ref"] = $"#/definitions/{memberType.FullName}" };
                    }
                    else
                    {
                        propertyInfo = GenerateJsonSchema(memberType, rootSchema);
                    }

                    // override properties with values from function property attribute
                    if (functionPropertyAttribute != null)
                    {
                        propertyInfo["description"] = functionPropertyAttribute.Description;

                        if (functionPropertyAttribute.Required)
                        {
                            requiredMembers.Add(propertyName);
                        }

                        JToken defaultValue = null;

                        if (functionPropertyAttribute.DefaultValue != null)
                        {
                            defaultValue = JToken.FromObject(functionPropertyAttribute.DefaultValue, serializer);
                            propertyInfo["default"] = defaultValue;
                        }

                        if (functionPropertyAttribute.PossibleValues is { Length: > 0 })
                        {
                            var enums = new JArray();

                            foreach (var value in functionPropertyAttribute.PossibleValues)
                            {
                                var @enum = JToken.FromObject(value, serializer);

                                if (defaultValue == null)
                                {
                                    enums.Add(@enum);
                                }
                                else
                                {
                                    if (@enum != defaultValue)
                                    {
                                        enums.Add(@enum);
                                    }
                                }
                            }

                            if (defaultValue != null && !enums.Contains(defaultValue))
                            {
                                enums.Add(defaultValue);
                            }

                            propertyInfo["enum"] = enums;
                        }
                    }
                    else if (jsonPropertyAttribute != null)
                    {
                        switch (jsonPropertyAttribute.Required)
                        {
                            case Required.Default:
                            case Required.Always:
                            case Required.AllowNull:
                                requiredMembers.Add(propertyName);
                                break;
                            case Required.DisallowNull:
                            default:
                                requiredMembers.Remove(propertyName);
                                break;
                        }
                    }
                    else if (Nullable.GetUnderlyingType(memberType) == null)
                    {
                        if (member is FieldInfo)
                        {
                            requiredMembers.Add(propertyName);
                        }
                    }

                    memberInfo[propertyName] = propertyInfo;
                }

                schema["properties"] = memberInfo;

                if (requiredMembers.Count > 0)
                {
                    schema["required"] = requiredMembers;
                }

                schema["additionalProperties"] = false;
                rootSchema["definitions"] ??= new JObject();
                rootSchema["definitions"][type.FullName] = schema;
                return new JObject { ["$ref"] = $"#/definitions/{type.FullName}" };
            }

            return schema;
        }

        private static bool TryGetSimpleTypeSchema(this Type type, out string schemaType)
        {
            switch (type)
            {
                case not null when type == typeof(object):
                    schemaType = "object";
                    return true;
                case not null when type == typeof(bool):
                    schemaType = "boolean";
                    return true;
                case not null when type == typeof(float) ||
                                   type == typeof(double) ||
                                   type == typeof(decimal):
                    schemaType = "number";
                    return true;
                case not null when type == typeof(char) ||
                                   type == typeof(string) ||
                                   type == typeof(Guid) ||
                                   type == typeof(DateTime) ||
                                   type == typeof(DateTimeOffset):
                    schemaType = "string";
                    return true;
                case not null when type == typeof(int) ||
                                   type == typeof(long) ||
                                   type == typeof(uint) ||
                                   type == typeof(byte) ||
                                   type == typeof(sbyte) ||
                                   type == typeof(ulong) ||
                                   type == typeof(short) ||
                                   type == typeof(ushort):
                    schemaType = "integer";
                    return true;
                default:
                    schemaType = null;
                    return false;
            }
        }

        private static bool TryGetDictionaryValueType(this Type type, out Type valueType)
        {
            valueType = null;

            if (!type.IsGenericType) { return false; }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            if (genericTypeDefinition == typeof(Dictionary<,>) ||
                genericTypeDefinition == typeof(IDictionary<,>) ||
                genericTypeDefinition == typeof(IReadOnlyDictionary<,>))
            {
                return InternalTryGetDictionaryValueType(type, out valueType);
            }

            // Check implemented interfaces for dictionary types
            foreach (var @interface in type.GetInterfaces())
            {
                if (!@interface.IsGenericType) { continue; }

                var interfaceTypeDefinition = @interface.GetGenericTypeDefinition();

                if (interfaceTypeDefinition == typeof(IDictionary<,>) ||
                    interfaceTypeDefinition == typeof(IReadOnlyDictionary<,>))
                {
                    return InternalTryGetDictionaryValueType(@interface, out valueType);
                }
            }

            return false;

            bool InternalTryGetDictionaryValueType(Type dictType, out Type dictValueType)
            {
                dictValueType = null;
                var genericArgs = dictType.GetGenericArguments();

                // The key type is not string, which cannot be represented in JSON object property names
                if (genericArgs[0] != typeof(string))
                {
                    throw new InvalidOperationException($"Cannot generate schema for dictionary type '{dictType.FullName}' with non-string key type.");
                }

                dictValueType = genericArgs[1].UnwrapNullableType();
                return true;
            }
        }

        private static readonly Type[] arrayTypes =
        {
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(IReadOnlyCollection<>),
            typeof(List<>),
            typeof(IList<>),
            typeof(IReadOnlyList<>),
            typeof(HashSet<>),
            typeof(ISet<>)
        };

        private static bool TryGetCollectionElementType(this Type type, out Type elementType)
        {
            elementType = null;

            if (type.IsArray)
            {
                elementType = type.GetElementType();
                return true;
            }

            if (!type.IsGenericType) { return false; }

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            if (!arrayTypes.Contains(genericTypeDefinition)) { return false; }

            elementType = type.GetGenericArguments()[0].UnwrapNullableType();
            return true;
        }

        private static Type UnwrapNullableType(this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        private static Type GetMemberType(MemberInfo member)
            => member switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                _ => throw new ArgumentException($"{nameof(MemberInfo)} must be of type {nameof(FieldInfo)}, {nameof(PropertyInfo)}", nameof(member))
            };
    }
}
