using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class DisabledVAD : IVoiceActivityDetectionSettings
    {
        [Preserve]
        public TurnDetectionType Type => TurnDetectionType.Disabled;

        public bool CreateResponse => false;

        public bool InterruptResponse => false;
    }
}
