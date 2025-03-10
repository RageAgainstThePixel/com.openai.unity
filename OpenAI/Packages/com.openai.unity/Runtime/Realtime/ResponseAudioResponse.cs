﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.Audio;

namespace OpenAI.Realtime
{
    [Preserve]
    public sealed class ResponseAudioResponse : BaseRealtimeEvent, IServerEvent, IRealtimeEventStream
    {
        [Preserve]
        [JsonConstructor]
        internal ResponseAudioResponse(
            [JsonProperty("event_id")] string eventId,
            [JsonProperty("type")] string type,
            [JsonProperty("response_id")] string responseId,
            [JsonProperty("item_id")] string itemId,
            [JsonProperty("output_index")] int outputIndex,
            [JsonProperty("content_index")] int contentIndex,
            [JsonProperty("delta")] string delta)
        {
            EventId = eventId;
            Type = type;
            ResponseId = responseId;
            ItemId = itemId;
            OutputIndex = outputIndex;
            ContentIndex = contentIndex;
            Delta = delta;
        }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("event_id")]
        public override string EventId { get; internal set; }

        /// <inheritdoc />
        [Preserve]
        [JsonProperty("type")]
        public override string Type { get; }

        /// <summary>
        /// The ID of the response.
        /// </summary>
        [Preserve]
        [JsonProperty("response_id")]
        public string ResponseId { get; }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        [Preserve]
        [JsonProperty("item_id")]
        public string ItemId { get; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [Preserve]
        [JsonProperty("output_index")]
        public int OutputIndex { get; }

        /// <summary>
        /// The index of the content part in the item's content array.
        /// </summary>
        [Preserve]
        [JsonProperty("content_index")]
        public int ContentIndex { get; }

        [Preserve]
        [JsonProperty("delta")]
        public string Delta { get; }

        [Preserve]
        [JsonIgnore]
        public float[] AudioSamples
            => audioSamples ??= PCMEncoder.Decode(Convert.FromBase64String(Delta), inputSampleRate: 24000, outputSampleRate: AudioSettings.outputSampleRate);
        private float[] audioSamples;

        [Preserve]
        [JsonIgnore]
        public float Length => AudioSamples.Length / (float)AudioSettings.outputSampleRate;

        [Preserve]
        [JsonIgnore]
        public AudioClip AudioClip
        {
            get
            {
                if (audioClip == null)
                {
                    audioClip = AudioClip.Create($"{ItemId}_{OutputIndex}_delta", AudioSamples.Length, 1, AudioSettings.outputSampleRate, false);
                    audioClip.SetData(AudioSamples, 0);
                }

                return audioClip;
            }
        }
        private AudioClip audioClip;

        [Preserve]
        [JsonIgnore]
        public bool IsDelta => Type.EndsWith("delta");

        [Preserve]
        [JsonIgnore]
        public bool IsDone => Type.EndsWith("done");

        [Preserve]
        public static implicit operator AudioClip(ResponseAudioResponse response) => response?.AudioClip;
    }
}
