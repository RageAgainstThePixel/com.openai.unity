// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using Utilities.WebRequestRest;

namespace OpenAI.Extensions
{
    internal static class ResponseExtensions
    {
        private const string Organization = "Openai-Organization";
        private const string RequestId = "X-Request-ID";
        private const string ProcessingTime = "Openai-Processing-Ms";

        internal static void SetResponseData(this BaseResponse response, Response restResponse)
        {
            if (restResponse is not { ResponseHeaders: not null }) { return; }

            if (restResponse.ResponseHeaders.TryGetValue(Organization, out var organization))
            {
                response.Organization = organization;
            }

            if (restResponse.ResponseHeaders.TryGetValue(RequestId, out var requestId))
            {
                response.RequestId = requestId;
            }

            if (restResponse.ResponseHeaders.TryGetValue(ProcessingTime, out var processingTime))
            {
                response.ProcessingTime = TimeSpan.FromMilliseconds(double.Parse(processingTime));
            }
        }

        internal static T DeserializeResponse<T>(this Response response, string json, JsonSerializerSettings settings) where T : BaseResponse
        {
            var result = JsonConvert.DeserializeObject<T>(json, settings);
            result.SetResponseData(response);
            return result;
        }
    }
}
