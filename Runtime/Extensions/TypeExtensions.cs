// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace OpenAI.Extensions
{
    internal static class TypeExtensions
    {
        public static JObject GenerateJsonSchema(this MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            if (parameters.Length == 0)
            {
                return null;
            }

            var schema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject()
            };
            var requiredParameters = new JArray();

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType == typeof(CancellationToken))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(parameter.Name))
                {
                    throw new InvalidOperationException($"Failed to find a valid parameter name for {methodInfo.DeclaringType}.{methodInfo.Name}()");
                }

                if (!parameter.HasDefaultValue)
                {
                    requiredParameters.Add(parameter.Name);
                }

                schema["properties"]![parameter.Name] = GenerateJsonSchema(parameter.ParameterType, schema);

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

            return schema;
        }

        public static JObject GenerateJsonSchema(this Type type, JObject rootSchema)
        {
            var schema = new JObject();

            if (!type.IsPrimitive &&
                type != typeof(Guid) &&
                type != typeof(DateTime) &&
                type != typeof(DateTimeOffset) &&
                rootSchema["definitions"] != null &&
                ((JObject)rootSchema["definitions"]).ContainsKey(type.FullName!))
            {
                return new JObject { ["$ref"] = $"#/definitions/{type.FullName}" };
            }

            if (type == typeof(string))
            {
                schema["type"] = "string";
            }
            else if (type == typeof(int) ||
                     type == typeof(long) ||
                     type == typeof(uint) ||
                     type == typeof(byte) ||
                     type == typeof(sbyte) ||
                     type == typeof(ulong) ||
                     type == typeof(short) ||
                     type == typeof(ushort))
            {
                schema["type"] = "integer";
            }
            else if (type == typeof(float) ||
                     type == typeof(double) ||
                     type == typeof(decimal))
            {
                schema["type"] = "number";
            }
            else if (type == typeof(bool))
            {
                schema["type"] = "boolean";
            }
            else if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                schema["type"] = "string";
                schema["format"] = "date-time";
            }
            else if (type == typeof(Guid))
            {
                schema["type"] = "string";
                schema["format"] = "uuid";
            }
            else if (type.IsEnum)
            {
                schema["type"] = "string";
                schema["enum"] = new JArray();

                foreach (var value in Enum.GetValues(type))
                {
                    ((JArray)schema["enum"]).Add(JToken.FromObject(value, OpenAIClient.JsonSerializer));
                }
            }
            else if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                schema["type"] = "array";
                var elementType = type.GetElementType() ?? type.GetGenericArguments()[0];

                if (rootSchema["definitions"] != null &&
                         ((JObject)rootSchema["definitions"]).ContainsKey(elementType.FullName))
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
                rootSchema["definitions"][type.FullName] = new JObject();

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
                            ((JObject)rootSchema["definitions"]).ContainsKey(memberType.FullName))
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
                            defaultValue = JToken.FromObject(functionPropertyAttribute.DefaultValue, OpenAIClient.JsonSerializer);
                            propertyInfo["default"] = defaultValue;
                        }

                        if (functionPropertyAttribute.PossibleValues is { Length: > 0 })
                        {
                            var enums = new JArray();

                            foreach (var value in functionPropertyAttribute.PossibleValues)
                            {
                                var @enum = JToken.FromObject(value, OpenAIClient.JsonSerializer);

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
                            case Required.Always:
                            case Required.AllowNull:
                                requiredMembers.Add(propertyName);
                                break;
                            case Required.Default:
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

                rootSchema["definitions"] ??= new JObject();
                rootSchema["definitions"][type.FullName] = schema;
                return new JObject { ["$ref"] = $"#/definitions/{type.FullName}" };
            }

            return schema;
        }

        private static Type GetMemberType(MemberInfo member)
            => member switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                _ => throw new ArgumentException($"{nameof(MemberInfo)} must be of type {nameof(FieldInfo)}, {nameof(PropertyInfo)}", nameof(member))
            };
    }
}
