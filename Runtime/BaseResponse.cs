// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace OpenAI
{
    public abstract class BaseResponse
    {
        /// <summary>
        /// The server-side processing time as reported by the API.  This can be useful for debugging where a delay occurs.
        /// </summary>
        public TimeSpan ProcessingTime { get; internal set; }

        /// <summary>
        /// The organization associated with the API request, as reported by the API.
        /// </summary>
        public string Organization { get; internal set; }

        /// <summary>
        /// The request id of this API call, as reported in the response headers.  This may be useful for troubleshooting or when contacting OpenAI support in reference to a specific request.
        /// </summary>
        public string RequestId { get; internal set; }
    }
}
