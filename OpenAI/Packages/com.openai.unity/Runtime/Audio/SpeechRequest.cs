// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using System;
using UnityEngine.Scripting;

namespace OpenAI.Audio
{
    [Preserve]
    public sealed class SpeechRequest
    {
        [Obsolete("use new .ctr overload with instructions parameter")]
        public SpeechRequest(string input, Model model, Voice voice, SpeechResponseFormat responseFormat, float? speed)
        {
            Input = !string.IsNullOrWhiteSpace(input) ? input : throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            Model = string.IsNullOrWhiteSpace(model?.Id) ? Models.Model.TTS_1 : model;
            Voice = string.IsNullOrWhiteSpace(voice?.Id) ? OpenAI.Voice.Alloy : voice;
            ResponseFormat = responseFormat;
            Speed = speed;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">The text to generate audio for. The maximum length is 4096 characters.</param>
        /// <param name="model">One of the available TTS models. Defaults to tts-1.</param>
        /// <param name="voice">The voice to use when generating the audio.</param>
        /// <param name="instructions">Control the voice of your generated audio with additional instructions. Does not work with tts-1 or tts-1-hd.</param>
        /// <param name="responseFormat">The format to audio in. Supported formats are mp3, opus, aac, flac, wav and pcm.</param>
        /// <param name="speed">The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.</param>
        [Preserve]
        public SpeechRequest(string input, Model model = null, Voice voice = null, string instructions = null, SpeechResponseFormat responseFormat = SpeechResponseFormat.MP3, float? speed = null)
        {
            Input = !string.IsNullOrWhiteSpace(input) ? input : throw new ArgumentException("Input cannot be null or empty.", nameof(input));
            Model = string.IsNullOrWhiteSpace(model?.Id) ? Models.Model.TTS_1 : model;
            Voice = string.IsNullOrWhiteSpace(voice?.Id) ? OpenAI.Voice.Alloy : voice;
            Instructions = instructions;
            ResponseFormat = responseFormat;
            Speed = speed;
        }

        /// <summary>
        /// One of the available TTS models. Defaults to tts-1.
        /// </summary>
        [Preserve]
        [JsonProperty("model")]
        [FunctionProperty("One of the available TTS models. Defaults to tts-1.", true, "tts-1", "tts-1-hd")]
        public string Model { get; }

        /// <summary>
        /// The text to generate audio for. The maximum length is 4096 characters.
        /// </summary>
        [Preserve]
        [JsonProperty("input")]
        [FunctionProperty("The text to generate audio for. The maximum length is 4096 characters.", true)]
        public string Input { get; }

        /// <summary>
        /// The voice to use when generating the audio.
        /// </summary>
        [Preserve]
        [JsonProperty("voice", DefaultValueHandling = DefaultValueHandling.Include)]
        [FunctionProperty("The voice to use when generating the audio.", true, "alloy", "echo", "fable", "onyx", "nova", "shimmer")]
        public string Voice { get; }

        /// <summary>
        /// Control the voice of your generated audio with additional instructions. Does not work with tts-1 or tts-1-hd.
        /// </summary>
        [Preserve]
        [JsonProperty("instructions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [FunctionProperty("Control the voice of your generated audio with additional instructions. Does not work with tts-1 or tts-1-hd.")]
        public string Instructions { get; }

        /// <summary>
        /// The format to audio in. Supported formats are mp3, opus, aac, flac, wav and pcm.
        /// </summary>
        [Preserve]
        [JsonProperty("response_format", DefaultValueHandling = DefaultValueHandling.Include)]
        [FunctionProperty("The format to audio in. Supported formats are mp3, opus, aac, flac, wav and pcm.", false, SpeechResponseFormat.MP3)]
        public SpeechResponseFormat ResponseFormat { get; internal set; }

        /// <summary>
        /// The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.
        /// </summary>
        [Preserve]
        [JsonProperty("speed")]
        [FunctionProperty("The speed of the generated audio. Select a value from 0.25 to 4.0. 1.0 is the default.", false, 1.0f)]
        public float? Speed { get; }
    }
}
