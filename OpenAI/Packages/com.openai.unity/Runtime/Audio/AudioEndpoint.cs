// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Newtonsoft.Json;
using OpenAI.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities.WebRequestRest;

namespace OpenAI.Audio
{
    /// <summary>
    /// Transforms audio into text.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/audio"/>
    /// </summary>
    public sealed class AudioEndpoint : OpenAIBaseEndpoint
    {
        [Preserve]
        private class AudioResponse
        {
            [Preserve]
            [JsonConstructor]
            public AudioResponse([JsonProperty("text")] string text)
            {
                Text = text;
            }

            [Preserve]
            [JsonProperty("text")]
            public string Text { get; }
        }

        /// <inheritdoc />
        public AudioEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "audio";

        /// <summary>
        /// Generates audio from the input text.
        /// </summary>
        /// <param name="request"><see cref="SpeechRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns><see cref="AudioClip"/> and the cached path.</returns>
        public async Task<Tuple<string, AudioClip>> CreateSpeechAsync(SpeechRequest request, CancellationToken cancellationToken = default)
        {
            var audioFormat = request.ResponseFormat switch
            {
                SpeechResponseFormat.MP3 => AudioType.MPEG,
                _ => throw new NotSupportedException(request.ResponseFormat.ToString())
            };
            var ext = request.ResponseFormat switch
            {
                SpeechResponseFormat.MP3 => "mp3",
                _ => throw new NotSupportedException(request.ResponseFormat.ToString())
            };
            var payload = JsonConvert.SerializeObject(request, OpenAIClient.JsonSerializationOptions);
            var response = await Rest.PostAsync(GetUrl("/speech"), payload, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            await Rest.ValidateCacheDirectoryAsync();
            var cacheDirectory = Rest.DownloadCacheDirectory
                  .CreateNewDirectory(nameof(OpenAI)
                  .CreateNewDirectory(nameof(Audio)
                  .CreateNewDirectory("Speech")));
            var cachedPath = Path.Combine(cacheDirectory, $"{DateTime.UtcNow:yyyyMMddThhmmss}.{ext}");
            await File.WriteAllBytesAsync(cachedPath, response.Data, cancellationToken).ConfigureAwait(true);
            var clip = await Rest.DownloadAudioClipAsync($"file://{cachedPath}", audioFormat, cancellationToken: cancellationToken);
            return new Tuple<string, AudioClip>(cachedPath, clip);
        }

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"><see cref="AudioTranscriptionRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>The transcribed text.</returns>
        public async Task<string> CreateTranscriptionAsync(AudioTranscriptionRequest request, CancellationToken cancellationToken = default)
        {
            var form = new WWWForm();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            form.AddBinaryData("file", audioData.ToArray(), request.AudioName);
            form.AddField("model", request.Model);

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                form.AddField("prompt", request.Prompt);
            }

            var responseFormat = request.ResponseFormat;
            form.AddField("response_format", responseFormat.ToString().ToLower());

            if (request.Temperature.HasValue)
            {
                form.AddField("temperature", request.Temperature.ToString());
            }

            if (!string.IsNullOrWhiteSpace(request.Language))
            {
                form.AddField("language", request.Language);
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/transcriptions"), form, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(response.Body)?.Text
                : response.Body;
        }

        /// <summary>
        /// Translates audio into English.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The translated text.</returns>
        public async Task<string> CreateTranslationAsync(AudioTranslationRequest request, CancellationToken cancellationToken = default)
        {
            var form = new WWWForm();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            form.AddBinaryData("file", audioData.ToArray(), request.AudioName);
            form.AddField("model", request.Model);

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                form.AddField("prompt", request.Prompt);
            }

            var responseFormat = request.ResponseFormat;
            form.AddField("response_format", responseFormat.ToString().ToLower());

            if (request.Temperature.HasValue)
            {
                form.AddField("temperature", request.Temperature.ToString());
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/translations"), form, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.Validate(EnableDebug);
            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(response.Body)?.Text
                : response.Body;
        }
    }
}
