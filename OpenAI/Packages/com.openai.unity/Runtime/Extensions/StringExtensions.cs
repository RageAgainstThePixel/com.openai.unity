// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http;
using System.Text;

namespace OpenAI.Extensions
{
    internal static class StringExtensions
    {
        public const string StreamData = "data: ";
        public const string StreamDone = "[DONE]";

        /// <summary>
        /// Attempts to get the event data from the string data.
        /// Returns false once the stream is done.
        /// </summary>
        /// <param name="streamData">Raw stream data.</param>
        /// <param name="eventData">Parsed stream data.</param>
        /// <returns>True, if the stream is not done. False if stream is done.</returns>
        public static bool TryGetEventStreamData(this string streamData, out string eventData)
        {
            eventData = string.Empty;

            if (streamData.StartsWith(StreamData))
            {
                eventData = streamData[StreamData.Length..].Trim();
            }

            return eventData != StreamDone;
        }

        public static StringContent ToJsonStringContent(this string json)
            => new StringContent(json, Encoding.UTF8, "application/json");
    }
}
