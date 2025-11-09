using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public interface IVoiceActivityDetectionSettings
    {
        public TurnDetectionType Type { get; }
        public bool? CreateResponse { get; }
        public bool? InterruptResponse { get; }
    }
}
