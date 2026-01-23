// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class RealtimeAudioInputConfig
    {
        [Preserve]
        [JsonConstructor]
        public RealtimeAudioInputConfig(
            [JsonProperty("format")] RealtimeAudioFormatConfig format = null,
            [JsonProperty("transcription")] InputAudioTranscriptionSettings transcription = null,
            [JsonProperty("noise_reduction")] NoiseReductionSettings noiseReduction = null,
            [JsonProperty("turn_detection")][JsonConverter(typeof(VoiceActivityDetectionSettingsConverter))] IVoiceActivityDetectionSettings turnDetection = null)
        {
            Format = format ?? new RealtimeAudioFormatConfig();
            Transcription = transcription;
            NoiseReduction = noiseReduction;
            TurnDetection = turnDetection;
        }

        /// <summary>
        /// Input audio format configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("format", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RealtimeAudioFormatConfig Format { get; }

        /// <summary>
        /// Optional asynchronous transcription settings.
        /// </summary>
        [Preserve]
        [JsonProperty("transcription", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputAudioTranscriptionSettings Transcription { get; }

        /// <summary>
        /// Optional noise reduction settings.
        /// </summary>
        [Preserve]
        [JsonProperty("noise_reduction", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NoiseReductionSettings NoiseReduction { get; }

        /// <summary>
        /// Optional turn detection configuration.
        /// </summary>
        [Preserve]
        [JsonProperty("turn_detection", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(VoiceActivityDetectionSettingsConverter))]
        public IVoiceActivityDetectionSettings TurnDetection { get; }
    }
}
