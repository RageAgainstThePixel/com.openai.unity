// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public abstract class BaseRealtimeEvent : IRealtimeEvent
    {
        /// <inheritdoc />
        public abstract string EventId { get; internal set; }

        /// <inheritdoc />
        public abstract string Type { get; }

        /// <inheritdoc />
        [Preserve]
        public virtual string ToJsonString()
            => JsonConvert.SerializeObject(this, OpenAIClient.JsonSerializationOptions);
    }
}
