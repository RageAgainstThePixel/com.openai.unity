// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAI.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Async;

namespace OpenAI
{
    /// <summary>
    /// <see href="https://platform.openai.com/docs/guides/function-calling"/>
    /// </summary>
    [Preserve]
    public sealed class Function
    {
        private const string NameRegex = "^[a-zA-Z0-9_-]{1,64}$";

        [Preserve]
        public Function() { }

        /// <summary>
        /// Creates a new function description to insert into a chat conversation.
        /// </summary>
        /// <param name="name">
        /// Required. The name of the function to generate arguments for based on the context in a message.<br/>
        /// May contain a-z, A-Z, 0-9, underscores and dashes, with a maximum length of 64 characters. Recommended to not begin with a number or a dash.
        /// </param>
        /// <param name="description">
        /// An optional description of the function, used by the API to determine if it is useful to include in the response.
        /// </param>
        /// <param name="parameters">
        /// An optional JSON object describing the parameters of the function that the model can generate.
        /// </param>
        [Preserve]
        public Function(string name, string description = null, JToken parameters = null)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, NameRegex))
            {
                throw new ArgumentException($"The name of the function does not conform to naming standards: {NameRegex}");
            }

            Name = name;
            Description = description;
            Parameters = parameters;
            functionCache[Name] = this;
        }

        /// <summary>
        /// Creates a new function description to insert into a chat conversation.
        /// </summary>
        /// <param name="name">
        /// Required. The name of the function to generate arguments for based on the context in a message.<br/>
        /// May contain a-z, A-Z, 0-9, underscores and dashes, with a maximum length of 64 characters. Recommended to not begin with a number or a dash.
        /// </param>
        /// <param name="description">
        /// An optional description of the function, used by the API to determine if it is useful to include in the response.
        /// </param>
        /// <param name="parameters">
        /// An optional JSON describing the parameters of the function that the model can generate.
        /// </param>
        [Preserve]
        public Function(string name, string description, string parameters)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, NameRegex))
            {
                throw new ArgumentException($"The name of the function does not conform to naming standards: {NameRegex}");
            }

            Name = name;
            Description = description;
            Parameters = new JObject(parameters);
            functionCache[Name] = this;
        }

        [Preserve]
        internal Function(string name, string description, MethodInfo method, object instance = null)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, NameRegex))
            {
                throw new ArgumentException($"The name of the function does not conform to naming standards: {NameRegex}");
            }

            Name = name;
            Description = description;
            MethodInfo = method;
            Parameters = method.GenerateJsonSchema();
            Instance = instance;
            functionCache[Name] = this;
        }

        #region Func<,> Overloads

        public static Function FromFunc<TResult>(string name, Func<TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, TResult>(string name, Func<T1, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, TResult>(string name, Func<T1, T2, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, TResult>(string name, Func<T1, T2, T3, TResult> function, string description = null)
        => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, TResult>(string name, Func<T1, T2, T3, T4, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, TResult>(string name, Func<T1, T2, T3, T4, T5, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, T7, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        public static Function FromFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(string name, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, string description = null)
            => new(name, description, function.Method, function.Target);

        #endregion Func<,> Overloads

        [Preserve]
        internal Function(Function other) => CopyFrom(other);

        /// <summary>
        /// The name of the function to generate arguments for.<br/>
        /// May contain a-z, A-Z, 0-9, and underscores and dashes, with a maximum length of 64 characters.
        /// Recommended to not begin with a number or a dash.
        /// </summary>
        [Preserve]
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// The optional description of the function.
        /// </summary>
        [Preserve]
        [JsonProperty("description")]
        public string Description { get; private set; }

        private string parametersString;

        private JToken parameters;

        /// <summary>
        /// The optional parameters of the function.
        /// Describe the parameters that the model should generate in JSON schema format (json-schema.org).
        /// </summary>
        [Preserve]
        [JsonProperty("parameters")]
        public JToken Parameters
        {
            get
            {
                if (parameters == null &&
                    !string.IsNullOrWhiteSpace(parametersString))
                {
                    parameters = JToken.Parse(parametersString);
                }

                return parameters;
            }
            private set => parameters = value;
        }

        private string argumentsString;

        private JToken arguments;

        /// <summary>
        /// The arguments to use when calling the function.
        /// </summary>
        [Preserve]
        [JsonProperty("arguments")]
        public JToken Arguments
        {
            get
            {
                if (arguments == null &&
                    !string.IsNullOrWhiteSpace(argumentsString))
                {
                    arguments = JToken.FromObject(argumentsString, JsonSerializer.Create(OpenAIClient.JsonSerializationOptions));
                }

                return arguments;
            }
            internal set => arguments = value;
        }

        /// <summary>
        /// The instance of the object to invoke the method on.
        /// </summary>
        [Preserve]
        [JsonIgnore]
        internal object Instance { get; }

        /// <summary>
        /// The method to invoke.
        /// </summary>
        [Preserve]
        [JsonIgnore]
        private MethodInfo MethodInfo { get; }

        [Preserve]
        internal void CopyFrom(Function other)
        {
            if (!string.IsNullOrWhiteSpace(other.Name))
            {
                Name = other.Name;
            }

            if (!string.IsNullOrWhiteSpace(other.Description))
            {
                Description = other.Description;
            }

            if (other.Arguments != null)
            {
                argumentsString += other.Arguments.ToString();
            }

            if (other.Parameters != null)
            {
                parametersString += other.Parameters.ToString();
            }
        }

        #region Function Invoking Utilities

        private static readonly ConcurrentDictionary<string, Function> functionCache = new();

        /// <summary>
        /// Invokes the function and returns the result as json.
        /// </summary>
        /// <returns>The result of the function as json.</returns>
        [Preserve]
        public string Invoke()
        {
            try
            {
                var (function, invokeArgs) = ValidateFunctionArguments();

                if (function.MethodInfo.ReturnType == typeof(void))
                {
                    function.MethodInfo.Invoke(function.Instance, invokeArgs);
                    return "{\"result\": \"success\"}";
                }

                var result = Invoke<object>();
                return JsonConvert.SerializeObject(new { result }, OpenAIClient.JsonSerializationOptions);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return JsonConvert.SerializeObject(new { error = e.Message }, OpenAIClient.JsonSerializationOptions);
            }
        }

        /// <summary>
        /// Invokes the function and returns the result.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <returns>The result of the function.</returns>
        [Preserve]
        public T Invoke<T>()
        {
            try
            {
                var (function, invokeArgs) = ValidateFunctionArguments();
                var result = function.MethodInfo.Invoke(function.Instance, invokeArgs);
                return result == null ? default : (T)result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        /// <summary>
        /// Invokes the function and returns the result as json.
        /// </summary>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>The result of the function as json.</returns>
        [Preserve]
        public async Task<string> InvokeAsync(CancellationToken cancellationToken = default)
        {
            await Awaiters.UnityMainThread;

            try
            {
                var (function, invokeArgs) = ValidateFunctionArguments(cancellationToken);

                if (function.MethodInfo.ReturnType == typeof(Task))
                {
                    if (function.MethodInfo.Invoke(function.Instance, invokeArgs) is not Task task)
                    {
                        throw new InvalidOperationException($"The function {Name} did not return a valid Task.");
                    }

                    await task;
                    return "{\"result\": \"success\"}";
                }

                var result = await InvokeAsync<object>(cancellationToken);
                return JsonConvert.SerializeObject(new { result }, OpenAIClient.JsonSerializationOptions);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return JsonConvert.SerializeObject(new { error = e.Message }, OpenAIClient.JsonSerializationOptions);
            }
        }

        /// <summary>
        /// Invokes the function and returns the result.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>The result of the function.</returns>
        [Preserve]
        public async Task<T> InvokeAsync<T>(CancellationToken cancellationToken = default)
        {
            await Awaiters.UnityMainThread;

            try
            {
                var (function, invokeArgs) = ValidateFunctionArguments(cancellationToken);

                if (function.MethodInfo.Invoke(function.Instance, invokeArgs) is not Task task)
                {
                    throw new InvalidOperationException($"The function {Name} did not return a valid Task.");
                }

                await task;
                // ReSharper disable once InconsistentNaming
                const string Result = nameof(Result);
                var resultProperty = task.GetType().GetProperty(Result);
                return (T)resultProperty?.GetValue(task);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        private (Function function, object[] invokeArgs) ValidateFunctionArguments(CancellationToken cancellationToken = default)
        {
            if (Parameters is { HasValues: true } && Arguments == null)
            {
                throw new ArgumentException($"Function {Name} has parameters but no arguments are set.");
            }

            if (!functionCache.TryGetValue(Name, out var function))
            {
                throw new InvalidOperationException($"Failed to find a valid function for {Name}");
            }

            if (function.MethodInfo == null)
            {
                throw new InvalidOperationException($"Failed to find a valid method for {Name}");
            }

            var requestedArgs = Arguments != null
                ? JsonConvert.DeserializeObject<Dictionary<string, object>>(Arguments.ToString(), OpenAIClient.JsonSerializationOptions)
                : new();
            var methodParams = function.MethodInfo.GetParameters();
            var invokeArgs = new object[methodParams.Length];

            for (var i = 0; i < methodParams.Length; i++)
            {
                var parameter = methodParams[i];

                if (parameter.Name == null)
                {
                    throw new InvalidOperationException($"Failed to find a valid parameter name for {function.MethodInfo.DeclaringType}.{function.MethodInfo.Name}()");
                }

                if (requestedArgs.TryGetValue(parameter.Name, out var value))
                {
                    if (parameter.ParameterType == typeof(CancellationToken))
                    {
                        invokeArgs[i] = cancellationToken;
                    }
                    else if (value is string @enum && parameter.ParameterType.IsEnum)
                    {
                        invokeArgs[i] = Enum.Parse(parameter.ParameterType, @enum, true);
                    }
                    else if (value is JObject json)
                    {
                        invokeArgs[i] = json.ToObject(parameter.ParameterType, JsonSerializer.Create(OpenAIClient.JsonSerializationOptions));
                    }
                    else
                    {
                        invokeArgs[i] = value;
                    }
                }
                else if (parameter.HasDefaultValue)
                {
                    invokeArgs[i] = parameter.DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"Missing argument for parameter '{parameter.Name}'");
                }
            }

            return (function, invokeArgs);
        }

        #endregion Function Invoking Utilities
    }
}
