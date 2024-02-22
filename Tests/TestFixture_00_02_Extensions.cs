// Licensed under the MIT License. See LICENSE in the project root for license information.

using NUnit.Framework;
using System.Text;
using UnityEngine;

namespace OpenAI.Tests
{
    internal class TestFixture_00_02_Extensions
    {
        [Test]
        public void Test_01_Tools()
        {
            var tools = Tool.GetAllAvailableTools();
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < tools.Count; i++)
            {
                var tool = tools[i];

                stringBuilder.Append(tool.Type != "function" ? $"  \"{tool.Type}\"" : $"  \"{tool.Function.Name}\"");

                if (tool.Function?.Parameters != null)
                {
                    stringBuilder.Append($": {tool.Function.Parameters}");
                }

                if (i < tools.Count - 1)
                {
                    stringBuilder.Append(",\n");
                }
            }

            Debug.Log(stringBuilder.ToString());
        }
    }
}
