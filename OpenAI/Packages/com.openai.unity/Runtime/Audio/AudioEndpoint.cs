// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Rest.Extensions;

namespace OpenAI.Audio
{
    /// <summary>
    /// Transforms audio into text.<br/>
    /// <see href="https://platform.openai.com/docs/api-reference/audio"/>
    /// </summary>
    public sealed class AudioEndpoint : OpenAIBaseEndpoint
    {
        private class AudioResponse
        {
            public AudioResponse([JsonProperty("text")] string text)
            {
                Text = text;
            }

            [JsonProperty("text")]
            public string Text { get; }
        }

        /// <inheritdoc />
        public AudioEndpoint(OpenAIClient client) : base(client) { }

        /// <inheritdoc />
        protected override string Root => "audio";

        /// <summary>
        /// Transcribes audio into the input language.
        /// </summary>
        /// <param name="request"><see cref="AudioTranscriptionRequest"/>.</param>
        /// <param name="cancellationToken">Optional, <see cref="CancellationToken"/>.</param>
        /// <returns>The transcribed text.</returns>
        public async Task<string> CreateTranscriptionAsync(AudioTranscriptionRequest request, CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            content.Add(new ByteArrayContent(audioData.ToArray()), "file", request.AudioName);
            content.Add(new StringContent(request.Model), "model");

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                content.Add(new StringContent(request.Prompt), "prompt");
            }

            var responseFormat = request.ResponseFormat;
            content.Add(new StringContent(responseFormat.ToString().ToLower()), "response_format");

            if (request.Temperature.HasValue)
            {
                content.Add(new StringContent(request.Temperature.ToString()), "temperature");
            }

            if (!string.IsNullOrWhiteSpace(request.Language))
            {
                content.Add(new StringContent(request.Language), "language");
            }

            request.Dispose();

            var response = await client.Client.PostAsync(GetUrl("/transcriptions"), content, cancellationToken);
            var responseAsString = await response.ReadAsStringAsync();

            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(responseAsString)?.Text
                : responseAsString;
        }

        /// <summary>
        /// Translates audio into into English.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The translated text.</returns>
        public async Task<string> CreateTranslationAsync(AudioTranslationRequest request, CancellationToken cancellationToken = default)
        {
            using var content = new MultipartFormDataContent();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            content.Add(new ByteArrayContent(audioData.ToArray()), "file", request.AudioName);
            content.Add(new StringContent(request.Model), "model");

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                content.Add(new StringContent(request.Prompt), "prompt");
            }

            var responseFormat = request.ResponseFormat;
            content.Add(new StringContent(responseFormat.ToString().ToLower()), "response_format");

            if (request.Temperature.HasValue)
            {
                content.Add(new StringContent(request.Temperature.ToString()), "temperature");
            }

            request.Dispose();

            var response = await client.Client.PostAsync(GetUrl("/translations"), content, cancellationToken);
            var responseAsString = await response.ReadAsStringAsync();

            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(responseAsString)?.Text
                : responseAsString;
        }
    }
}
