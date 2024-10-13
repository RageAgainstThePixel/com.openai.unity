// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;

namespace OpenAI.Realtime
{
    public abstract class BaseRealtimeEventResponse
    {
        public string ToJsonString() => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);
    }
}
