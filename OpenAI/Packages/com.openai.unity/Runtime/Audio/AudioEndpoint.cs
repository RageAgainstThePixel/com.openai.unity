// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
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
            var wwwForm = new WWWForm();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            wwwForm.AddBinaryData("file", audioData.ToArray(), request.AudioName);
            wwwForm.AddField("model", request.Model);

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                wwwForm.AddField("prompt", request.Prompt);
            }

            var responseFormat = request.ResponseFormat;
            wwwForm.AddField("response_format", responseFormat.ToString().ToLower());

            if (request.Temperature.HasValue)
            {
                wwwForm.AddField("temperature", request.Temperature.ToString());
            }

            if (!string.IsNullOrWhiteSpace(request.Language))
            {
                wwwForm.AddField("language", request.Language);
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/transcriptions"), wwwForm, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.ValidateResponse();
            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(response.ResponseBody)?.Text
                : response.ResponseBody;
        }

        /// <summary>
        /// Translates audio into English.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The translated text.</returns>
        public async Task<string> CreateTranslationAsync(AudioTranslationRequest request, CancellationToken cancellationToken = default)
        {
            var wwwForm = new WWWForm();
            using var audioData = new MemoryStream();
            await request.Audio.CopyToAsync(audioData, cancellationToken);
            wwwForm.AddBinaryData("file", audioData.ToArray(), request.AudioName);
            wwwForm.AddField("model", request.Model);

            if (!string.IsNullOrWhiteSpace(request.Prompt))
            {
                wwwForm.AddField("prompt", request.Prompt);
            }

            var responseFormat = request.ResponseFormat;
            wwwForm.AddField("response_format", responseFormat.ToString().ToLower());

            if (request.Temperature.HasValue)
            {
                wwwForm.AddField("temperature", request.Temperature.ToString());
            }

            request.Dispose();

            var response = await Rest.PostAsync(GetUrl("/translations"), wwwForm, new RestParameters(client.DefaultRequestHeaders), cancellationToken);
            response.ValidateResponse();
            return responseFormat == AudioResponseFormat.Json
                ? JsonConvert.DeserializeObject<AudioResponse>(response.ResponseBody)?.Text
                : response.ResponseBody;
        }
    }
}
