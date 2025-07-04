// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json.Linq;

namespace OpenAI
{
    public interface IToolCall
    {
        public string CallId { get; }

        public string Name { get; }

        public JToken Arguments { get; }
    }
}
