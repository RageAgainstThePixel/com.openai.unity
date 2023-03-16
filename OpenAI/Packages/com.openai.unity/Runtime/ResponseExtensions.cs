// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenAI
{
    internal static class ResponseExtensions
    {
        private const string Organization = "Openai-Organization";
        private const string RequestId = "X-Request-ID";
        private const string ProcessingTime = "Openai-Processing-Ms";

        internal static void SetResponseData(this BaseResponse response, HttpResponseHeaders headers)
        {
            response.Organization = headers.GetValues(Organization).FirstOrDefault();
            response.RequestId = headers.GetValues(RequestId).FirstOrDefault();
            response.ProcessingTime = TimeSpan.FromMilliseconds(int.Parse(headers.GetValues(ProcessingTime).First()));
        }

        internal static async Task<string> ReadAsStringAsync(this HttpResponseMessage response, bool debug = false, [CallerMemberName] string methodName = null)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"{methodName} Failed! HTTP status code: {response.StatusCode} | Response body: {responseAsString}");
            }

            if (debug)
            {
                Debug.Log(responseAsString);
            }

            return responseAsString;
        }

        internal static async Task CheckResponseAsync(this HttpResponseMessage response, [CallerMemberName] string methodName = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseAsString = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"{methodName} Failed! HTTP status code: {response.StatusCode} | Response body: {responseAsString}");
            }
        }

        internal static T DeserializeResponse<T>(this HttpResponseMessage response, string json, JsonSerializerSettings settings) where T : BaseResponse
        {
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            result.SetResponseData(response.Headers);
            return result;
        }
    }
}
