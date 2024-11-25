// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.WebRequestRest;

namespace OpenAI.Audio
{
    /// <summary>
    /// Transforms audio into text.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/audio"/>
    /// </summary>
    public sealed class AudioEndpoint : OpenAIBaseEndpoint
    {
        internal AudioEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "audio";

        protected override bool? IsAzureDeployment => true;

        private static readonly object mutex = new();

        [Obsolete("use GetSpeechAsync")]
        public async Task<Tuple<string, AudioClip>> CreateSpeechAsync(SpeechRequest request, CancellationToken cancellationToken = default)
            => await CreateSpeechStreamAsync(request, null, cancellationToken);

        [Obsolete("use GetSpeechAsync")]
        public async Task<Tuple<string, AudioClip>> CreateSpeechStreamAsync(SpeechRequest request, Action<AudioClip> partialClipCallback, CancellationToken cancellationToken = default)
        {
            var result = await GetSpeechAsync(request, speechClip =>
            {
                partialClipCallback.Invoke(speechClip.AudioClip);
            }, cancellationToken);
            return Tuple.Create(result.CachePath, result.AudioClip);
        }

        /// <summary>
        /// Generates audio from the input text.
        /// </summary>
        /// <param name="request"><see cref="SpeechRequest"/>.</param>
        /// <param name="partialClipCallback">Optional, partial <see cref="SpeechClip"/> callback used to stream audio.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="SpeechClip"/></returns>
        [Function("Generates audio from the input text.")]
        public async Task<SpeechClip> GetSpeechAsync(SpeechRequest request, Action<SpeechClip> partialClipCallback = null, CancellationToken cancellationToken = default)
        {
            if (partialClipCallback != null && request.ResponseFormat != SpeechResponseFormat.PCM)
            {
                Debug.LogWarning("Speech streaming only supported with PCM response format. Overriding to PCM...");
                request.ResponseFormat = SpeechResponseFormat.PCM;
            }

            var ext = request.ResponseFormat switch
            {
                SpeechResponseFormat.MP3 => "mp3",
                SpeechResponseFormat.WAV => "wav",
                SpeechResponseFormat.PCM => "pcm",
                _ => throw new NotSupportedException(request.ResponseFormat.ToString())
            };
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            string clipName;

            lock (mutex)
            {
                clipName = $"{request.Voice}-{DateTime.UtcNow:yyyyMMddThhmmssfffff}.{ext}";
            }

            Rest.TryGetDownloadCacheItem(clipName, out var cachedPath);

            var part = 0;
            var pcmResponse = await Rest.PostAsync(GetUrl("/speech"), payload, StreamCallback, 8192, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            pcmResponse.Validate(EnableDebug);
            await File.WriteAllBytesAsync(cachedPath, pcmResponse.Data, cancellationToken).ConfigureAwait(true);
            return new SpeechClip(clipName, cachedPath, new ReadOnlyMemory<byte>(pcmResponse.Data));

            void StreamCallback(Response partialResponse)
            {
                partialClipCallback?.Invoke(new SpeechClip($"{clipName}_{++part}", null, partialResponse.Data));
            }
        }

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"><see cref="AudioTranscriptionRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>The transcribed text.</returns>
        public async Task<string> CreateTranscriptionTextAsync(AudioTranscriptionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await Internal_CreateTranscriptionAsync(request, cancellationToken);
            return request.ResponseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(response)?.Text
                : response;
        }

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <remarks>This method expects the request format to be either <see cref="AudioResponseFormat.Json"/> or <see cref="AudioResponseFormat.Verbose_Json"/>.</remarks>
        /// <param name="request"><see cref="AudioTranscriptionRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AudioResponse"/>.</returns>
        public async Task<AudioResponse> CreateTranscriptionJsonAsync(AudioTranscriptionRequest request, CancellationToken cancellationToken = default)
        {
            if (request.ResponseFormat is not (AudioResponseFormat.Json or AudioResponseFormat.Verbose_Json))
            {
                throw new ArgumentException("Response format must be Json or Verbose Json.", nameof(request.ResponseFormat));
            }

            var response = await Internal_CreateTranscriptionAsync(request, cancellationToken);
            return JsonConvert.DeserializeObject<AudioResponse>(response);
        }

        private async Task<string> Internal_CreateTranscriptionAsync(AudioTranscriptionRequest request, CancellationToken cancellationToken = default)
        {
            var payload = new WWWForm();

            try
            {
                using var audioData = new MemoryStream();
                await request.Audio.CopyToAsync(audioData, cancellationToken);
                payload.AddBinaryData("file", audioData.ToArray(), request.AudioName);
                payload.AddField("model", request.Model);

                if (!string.IsNullOrWhiteSpace(request.Prompt))
                {
                    payload.AddField("prompt", request.Prompt);
                }

                var responseFormat = request.ResponseFormat;
                payload.AddField("response_format", responseFormat.ToString().ToLower());

                if (request.Temperature.HasValue)
                {
                    payload.AddField("temperature", request.Temperature.Value.ToString(CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrWhiteSpace(request.Language))
                {
                    payload.AddField("language", request.Language);
                }

                switch (request.TimestampGranularities)
                {
                    case TimestampGranularity.Segment:
                    case TimestampGranularity.Word:
                        payload.AddField("timestamp_granularities[]", request.TimestampGranularities.ToString().ToLower());
                        break;
                }
            }
            finally
            {
                request.Dispose();
            }

            var response = await Rest.PostAsync(GetUrl("/transcriptions"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Body;
        }

        /// <summary>
        /// Translates audio into English.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The translated text.</returns>
        public async Task<string> CreateTranslationTextAsync(AudioTranslationRequest request, CancellationToken cancellationToken = default)
        {
            var responseAsString = await Internal_CreateTranslationAsync(request, cancellationToken);
            return request.ResponseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(responseAsString)?.Text
                : responseAsString;
        }

        /// <summary>
        /// Translates audio into English.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<AudioResponse> CreateTranslationJsonAsync(AudioTranslationRequest request, CancellationToken cancellationToken = default)
        {
            if (request.ResponseFormat is not (AudioResponseFormat.Json or AudioResponseFormat.Verbose_Json))
            {
                throw new ArgumentException("Response format must be Json or Verbose Json.", nameof(request.ResponseFormat));
            }

            var responseAsString = await Internal_CreateTranslationAsync(request, cancellationToken);
            return JsonConvert.DeserializeObject<AudioResponse>(responseAsString);
        }

        private async Task<string> Internal_CreateTranslationAsync(AudioTranslationRequest request, CancellationToken cancellationToken)
        {
            var payload = new WWWForm();

            try
            {
                using var audioData = new MemoryStream();
                await request.Audio.CopyToAsync(audioData, cancellationToken);
                payload.AddBinaryData("file", audioData.ToArray(), request.AudioName);
                payload.AddField("model", request.Model);

                if (!string.IsNullOrWhiteSpace(request.Prompt))
                {
                    payload.AddField("prompt", request.Prompt);
                }

                var responseFormat = request.ResponseFormat;
                payload.AddField("response_format", responseFormat.ToString().ToLower());

                if (request.Temperature.HasValue)
                {
                    payload.AddField("temperature", request.Temperature.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
            finally
            {
                request.Dispose();
            }

            var response = await Rest.PostAsync(GetUrl("/translations"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return response.Body;
        }
    }
}
