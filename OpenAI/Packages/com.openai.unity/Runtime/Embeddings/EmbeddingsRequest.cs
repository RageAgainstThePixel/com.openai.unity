// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace OpenAI.Embeddings
{
    [Preserve]
    public sealed class EmbeddingsRequest
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">
        /// Input text to get embeddings for, encoded as a string or array of tokens.
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays.
        /// Each input must not exceed 8192 tokens in length.
        /// </param>
        /// <param name="model">
        /// The <see cref="Models.Model"/> to use.
        /// Defaults to: <see cref="Model.Embedding_Ada_002"/>
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <exception cref="ArgumentNullException">A valid <see cref="input"/> string is a Required parameter.</exception>
        [Preserve]
        public EmbeddingsRequest(string input, string model = null, string user = null)
            : this(new List<string> { input }, model, user)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">
        /// Input text to get embeddings for, encoded as a string or array of tokens.
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays.
        /// Each input must not exceed 8192 tokens in length.
        /// </param>
        /// <param name="model">
        /// The model id to use.<br/>
        /// Defaults to: <see cref="Model.Embedding_Ada_002"/>
        /// </param>
        /// <param name="user">
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse.
        /// </param>
        /// <exception cref="ArgumentNullException">A valid <see cref="input"/> string is a Required parameter.</exception>
        [Preserve]
        public EmbeddingsRequest(IEnumerable<string> input, string model = null, string user = null)
        {
            Input = input?.ToList();

            if (Input?.Count == 0)
            {
                throw new ArgumentNullException(nameof(input), $"Missing required {nameof(input)} parameter");
            }

            Model = string.IsNullOrWhiteSpace(model) ? Models.Model.Embedding_Ada_002 : model;

            if (!Model.Contains("embedding") &&
                !Model.Contains("search") &&
                !Model.Contains("similarity"))
            {
                throw new ArgumentException($"{Model} is not supported", nameof(model));
            }

            User = user;
        }

        [Preserve]
        [JsonProperty("input")]
        public IReadOnlyList<string> Input { get; }

        [Preserve]
        [JsonProperty("model")]
        public string Model { get; }

        [Preserve]
        [JsonProperty("user")]
        public string User { get; }
    }
}
