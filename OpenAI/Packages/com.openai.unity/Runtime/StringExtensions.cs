// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Text;

namespace OpenAI
{
    internal static class StringExtensions
    {
        public static StringContent ToJsonStringContent(this string s)
            => new StringContent(s, Encoding.UTF8, "application/json");
    }
}
