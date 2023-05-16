// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OpenAI.Extensions
{
    internal static class ResponseExtensions
    {
        private const string Organization = "Openai-Organization";
        private const string RequestId = "X-Request-ID";
        private const string ProcessingTime = "Openai-Processing-Ms";

        internal static void SetResponseData(this BaseResponse response, HttpResponseHeaders headers)
        {
            if (headers.Contains(Organization))
            {
                response.Organization = headers.GetValues(Organization).FirstOrDefault();
            }

            response.ProcessingTime = TimeSpan.FromMilliseconds(double.Parse(headers.GetValues(ProcessingTime).First()));
            response.RequestId = headers.GetValues(RequestId).FirstOrDefault();
        }

        internal static T DeserializeResponse<T>(this HttpResponseMessage response, string json, JsonSerializerSettings settings) where T : BaseResponse
        {
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            result.SetResponseData(response.Headers);
            return result;
        }
    }
}
