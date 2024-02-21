// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public sealed class Tool
    {
        [Preserve]
        public Tool() { }

        [Preserve]
        public Tool(Tool other) => CopyFrom(other);

        [Preserve]
        public Tool(Function function)
        {
            Function = function;
            Type = nameof(function);
        }

        [Preserve]
        public static implicit operator Tool(Function function) => new(function);

        [Preserve]
        [JsonIgnore]
        public static Tool Retrieval { get; } = new() { Type = "retrieval" };

        [Preserve]
        [JsonIgnore]
        public static Tool CodeInterpreter { get; } = new() { Type = "code_interpreter" };

        [Preserve]
        [JsonProperty("id")]
        public string Id { get; private set; }

        [Preserve]
        [JsonProperty("index")]
        public int? Index { get; private set; }

        [Preserve]
        [JsonProperty("type")]
        public string Type { get; private set; }

        [Preserve]
        [JsonProperty("function")]
        public Function Function { get; private set; }

        [Preserve]
        internal void CopyFrom(Tool other)
        {
            if (!string.IsNullOrWhiteSpace(other?.Id))
            {
                Id = other.Id;
            }

            if (other is { Index: not null })
            {
                Index = other.Index.Value;
            }

            if (!string.IsNullOrWhiteSpace(other?.Type))
            {
                Type = other.Type;
            }

            if (other?.Function != null)
            {
                if (Function == null)
                {
                    Function = new Function(other.Function);
                }
                else
                {
                    Function.CopyFrom(other.Function);
                }
            }
        }

        [Preserve]
        public string InvokeFunction() => Function.Invoke();

        [Preserve]
        public async Task<string> InvokeFunctionAsync(CancellationToken cancellationToken = default)
            => await Function.InvokeAsync(cancellationToken);

        private static List<Tool> toolCache;

        [Preserve]
        public static IReadOnlyList<Tool> GetAllAvailableTools(bool includeDefaults = true)
        {
            if (toolCache != null)
            {
                return !includeDefaults
                    ? toolCache.Where(tool => tool.Type == "function").ToList()
                    : toolCache;
            }

            var tools = new List<Tool>();

            if (includeDefaults)
            {
                tools.Add(Retrieval);
                tools.Add(CodeInterpreter);
            }

            tools.AddRange(
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                from method in type.GetMethods()
                let functionAttribute = method.GetCustomAttribute<FunctionAttribute>()
                where functionAttribute != null
                let name = $"{type.FullName}.{method.Name}".Replace('.', '_')
                let description = functionAttribute.Description
                let parameters = method.GenerateJsonSchema()
                select new Function(name, description, parameters, method)
                into function
                select new Tool(function));
            toolCache = tools;
            return tools;
        }

        [Preserve]
        public static Tool GetOrCreateTool(Type type, string methodName)
        {
            var knownTools = GetAllAvailableTools(false);
            var method = type.GetMethod(methodName) ??
                throw new InvalidOperationException($"Failed to find a valid method for {type.FullName}.{methodName}()");
            var functionAttribute = method.GetCustomAttribute<FunctionAttribute>() ??
                throw new InvalidOperationException($"{type.FullName}.{methodName}() must have a {nameof(FunctionAttribute)} decorator!");
            var functionName = $"{type.FullName}.{method.Name}".Replace('.', '_');

            foreach (var knownTool in knownTools)
            {
                if (knownTool.Type == "function" && knownTool.Function.Name == functionName)
                {
                    return knownTool;
                }
            }

            var tool = new Tool(new Function(functionName, functionAttribute.Description, method.GenerateJsonSchema(), method));
            toolCache.Add(tool);
            return tool;
        }
    }
}
