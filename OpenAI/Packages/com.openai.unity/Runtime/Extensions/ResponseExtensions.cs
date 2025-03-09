// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Globalization;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Extensions
{
    internal static class ResponseExtensions
    {
        private const string RequestId = "X-Request-ID";
        private const string Organization = "Openai-Organization";
        private const string ProcessingTime = "Openai-Processing-Ms";
        private const string OpenAIVersion = "openai-version";
        private const string XRateLimitLimitRequests = "x-ratelimit-limit-requests";
        private const string XRateLimitLimitTokens = "x-ratelimit-limit-tokens";
        private const string XRateLimitRemainingRequests = "x-ratelimit-remaining-requests";
        private const string XRateLimitRemainingTokens = "x-ratelimit-remaining-tokens";
        private const string XRateLimitResetRequests = "x-ratelimit-reset-requests";
        private const string XRateLimitResetTokens = "x-ratelimit-reset-tokens";

        private static readonly NumberFormatInfo numberFormatInfo = new()
        {
            NumberGroupSeparator = ",",
            NumberDecimalSeparator = "."
        };

        internal static void SetResponseData(this BaseResponse response, Response restResponse, OpenAIClient client)
        {
            if (response is IListResponse<BaseResponse> listResponse)
            {
                foreach (var item in listResponse.Items)
                {
                    SetResponseData(item, restResponse, client);
                }
            }

            response.Client = client;

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

            if (restResponse.Headers.TryGetValue(OpenAIVersion, out var version))
            {
                response.OpenAIVersion = version;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitLimitRequests, out var limitRequests) &&
                int.TryParse(limitRequests, out var limitRequestsValue)
               )
            {
                response.LimitRequests = limitRequestsValue;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitLimitTokens, out var limitTokens) &&
                int.TryParse(limitTokens, out var limitTokensValue))
            {
                response.LimitTokens = limitTokensValue;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitRemainingRequests, out var remainingRequests) &&
                int.TryParse(remainingRequests, out var remainingRequestsValue))
            {
                response.RemainingRequests = remainingRequestsValue;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitRemainingTokens, out var remainingTokens) &&
                int.TryParse(remainingTokens, out var remainingTokensValue))
            {
                response.RemainingTokens = remainingTokensValue;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitResetRequests, out var resetRequests))
            {
                response.ResetRequests = resetRequests;
            }

            if (restResponse.Headers.TryGetValue(XRateLimitResetTokens, out var resetTokens))
            {
                response.ResetTokens = resetTokens;
            }
        }

        internal static T Deserialize<T>(this Response response, OpenAIClient client) where T : BaseResponse
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(response.Body, OpenAIClient.JsonSerializationOptions);
                result.SetResponseData(response, client);
                return result;
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to deserialize:\n{response.Body}");
                throw;
            }
        }
    }
}
