// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenAI.Images;
using OpenAI.Tests.StructuredOutput;
using OpenAI.Tests.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_00_02_Extensions : AbstractTestFixture
    {
        [Test]
        public void Test_01_01_GetTools()
        {
            var tools = Tool.GetAllAvailableTools(forceUpdate: true, clearCache: true).ToList();
            Assert.IsNotNull(tools);
            Assert.IsNotEmpty(tools);
            tools.Add(Tool.GetOrCreateTool(OpenAIClient.ImagesEndPoint, nameof(ImagesEndpoint.GenerateImageAsync)));
            tools.Add(Tool.FromFunc<GameObject, Vector2, Vector3, Quaternion, string>("complex_objects", (gameObject, vector2, vector3, quaternion) => "success"));
            var json = JsonConvert.SerializeObject(tools, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
            Debug.Log(json);
        }

        [Test]
        public async Task Test_01_02_Tool_Funcs()
        {
            var tools = new List<Tool>
            {
                Tool.FromFunc("test_func", Function),
                Tool.FromFunc<DateTime, Vector3, string>("test_func_with_args", FunctionWithArgs),
                Tool.FromFunc("test_func_weather", () => WeatherService.GetCurrentWeatherAsync("my location", WeatherService.WeatherUnit.Celsius)),
                Tool.FromFunc<List<int>, string>("test_func_with_array_args", FunctionWithArrayArgs),
                Tool.FromFunc<string, string>("test_single_return_arg", arg1 => arg1),
                Tool.FromFunc("test_no_specifiers", (string arg1) => arg1)
            };

            try
            {
                var json = JsonConvert.SerializeObject(tools, Formatting.Indented, OpenAIClient.JsonSerializationOptions);
                Debug.Log(json);
                Assert.IsNotNull(tools);
                var tool = tools[0];
                Assert.IsNotNull(tool);
                var toolCall = new ToolCall("toolCall_0", tool.Function.Name);
                var result = tool.InvokeFunction<string>(toolCall);
                Assert.AreEqual("success", result);

                var toolWithArgs = tools[1];
                Assert.IsNotNull(toolWithArgs);
                var testValue = new { arg1 = DateTime.UtcNow, arg2 = Vector3.one };
                toolCall = new ToolCall("toolCall_1", toolWithArgs.Function.Name, JToken.FromObject(testValue, OpenAIClient.JsonSerializer));
                var resultWithArgs = toolWithArgs.InvokeFunction<string>(toolCall);
                Debug.Log(resultWithArgs);

                var toolWeather = tools[2];
                Assert.IsNotNull(toolWeather);
                toolCall = new ToolCall("toolCall_2", toolWeather.Function.Name);
                var resultWeather = await toolWeather.InvokeFunctionAsync(toolCall);
                Assert.IsFalse(string.IsNullOrWhiteSpace(resultWeather));
                Debug.Log(resultWeather);

                var toolWithArrayArgs = tools[3];
                Assert.IsNotNull(toolWithArrayArgs);
                var arrayTestValue = new { list = new List<int> { 1, 2, 3, 4, 5 } };
                toolCall = new ToolCall("toolCall_3", toolWithArrayArgs.Function.Name, JToken.FromObject(arrayTestValue, OpenAIClient.JsonSerializer));
                var resultWithArrayArgs = toolWithArrayArgs.InvokeFunction<string>(toolCall);
                Debug.Log(resultWithArrayArgs);

                var toolSingleReturnArg = tools[4];
                Assert.IsNotNull(toolSingleReturnArg);
                toolCall = new ToolCall("toolCall_4", toolSingleReturnArg.Function.Name, JToken.FromObject(new Dictionary<string, string> { { "arg1", "arg1" } }, OpenAIClient.JsonSerializer));
                var resultSingleReturnArg = toolSingleReturnArg.InvokeFunction<string>(toolCall);
                Debug.Log(resultSingleReturnArg);
                Assert.AreEqual("arg1", resultSingleReturnArg);

                var toolNoSpecifiers = tools[5];
                Assert.IsNotNull(toolNoSpecifiers);
                toolCall = new ToolCall("toolCall_5", toolNoSpecifiers.Function.Name, JToken.FromObject(new Dictionary<string, string> { { "arg1", "arg1" } }, OpenAIClient.JsonSerializer));
                var resultNoSpecifiers = toolNoSpecifiers.InvokeFunction<string>(toolCall);
                Debug.Log(resultNoSpecifiers);
                Assert.AreEqual("arg1", resultNoSpecifiers);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private string Function()
        {
            return "success";
        }

        private string FunctionWithArgs(DateTime arg1, Vector3 arg2)
        {
            return JsonConvert.SerializeObject(new { arg1, arg2 }, OpenAIClient.JsonSerializationOptions);
        }

        private string FunctionWithArrayArgs(List<int> list)
        {
            return JsonConvert.SerializeObject(new { list }, OpenAIClient.JsonSerializationOptions);
        }

        [Test]
        public void Test_02_01_GenerateJsonSchema()
        {
            JsonSchema mathSchema = typeof(MathResponse);
            Debug.Log(mathSchema.ToString());
        }

        [Test]
        public void Test_02_02_GenerateJsonSchema_PrimitiveTypes()
        {
            JsonSchema schema = typeof(TestSchema);
            Debug.Log(schema.ToString());
        }

        private class TestSchema
        {
            // test all primitive types can be serialized
            public bool Bool { get; set; }
            public byte Byte { get; set; }
            public sbyte SByte { get; set; }
            public short Short { get; set; }
            public ushort UShort { get; set; }
            public int Integer { get; set; }
            public uint UInteger { get; set; }
            public long Long { get; set; }
            public ulong ULong { get; set; }
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
            public char Char { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public DateTimeOffset DateTimeOffset { get; set; }
            public Guid Guid { get; set; }
            // test nullables
            public int? NullInt { get; set; }
            public DateTime? NullDateTime { get; set; }
            public TestEnum TestEnum { get; set; }
            public TestEnum? NullEnum { get; set; }
            public Dictionary<string, object> Dictionary { get; set; }
            public IDictionary<string, int> IntDictionary { get; set; }
            public IReadOnlyDictionary<string, string> StringDictionary { get; set; }
            public Dictionary<string, MathResponse> CustomDictionary { get; set; }
        }

        private enum TestEnum
        {
            Enum1,
            Enum2,
            Enum3,
            Enum4
        }
    }
}
