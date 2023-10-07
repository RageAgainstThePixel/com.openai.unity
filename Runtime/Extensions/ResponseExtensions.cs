// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Globalization;
using Utilities.WebRequestRest;

namespace OpenAI.Extensions
{
    internal static class ResponseExtensions
    {
        private const string RequestId = "X-Request-ID";
        private const string Organization = "Openai-Organization";
        private const string ProcessingTime = "Openai-Processing-Ms";

        private static readonly NumberFormatInfo numberFormatInfo = new NumberFormatInfo
        {
            NumberGroupSeparator = ",",
            NumberDecimalSeparator = "."
        };

        internal static void SetResponseData(this BaseResponse response, Response restResponse)
        {
            if (restResponse is not { Headers: not null }) { return; }

            if (restResponse.Headers.TryGetValue(RequestId, out var requestId))
            {
                response.RequestId = requestId;
            }

            if (restResponse.Headers.TryGetValue(Organization, out var organization))
            {
                response.Organization = organization;
            }

            if (restResponse.Headers.TryGetValue(ProcessingTime, out var processingTimeString) &&
                double.TryParse(processingTimeString, NumberStyles.AllowDecimalPoint, numberFormatInfo, out var processingTime))
            {
                response.ProcessingTime = TimeSpan.FromMilliseconds(processingTime);
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
