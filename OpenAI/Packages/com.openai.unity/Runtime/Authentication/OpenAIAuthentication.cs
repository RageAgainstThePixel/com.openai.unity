// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Utilities.WebRequestRest.Interfaces;

namespace OpenAI
{
    /// <summary>
    /// Represents authentication for OpenAI
    /// </summary>
    public sealed class OpenAIAuthentication : AbstractAuthentication<OpenAIAuthentication, OpenAIAuthInfo, OpenAIConfiguration>
    {
        internal const string CONFIG_FILE = ".openai";
        private const string OPENAI_KEY = nameof(OPENAI_KEY);
        private const string OPENAI_API_KEY = nameof(OPENAI_API_KEY);
        private const string OPENAI_SECRET_KEY = nameof(OPENAI_SECRET_KEY);
        private const string TEST_OPENAI_SECRET_KEY = nameof(TEST_OPENAI_SECRET_KEY);
        private const string OPENAI_ORGANIZATION_ID = nameof(OPENAI_ORGANIZATION_ID);
        private const string OPEN_AI_ORGANIZATION_ID = nameof(OPEN_AI_ORGANIZATION_ID);
        private const string ORGANIZATION = nameof(ORGANIZATION);

        /// <summary>
        /// Allows implicit casting from a string, so that a simple string API key can be provided in place of an instance of <see cref="OpenAIAuthentication"/>.
        /// </summary>
        /// <param name="key">The API key to convert into a <see cref="OpenAIAuthentication"/>.</param>
        public static implicit operator OpenAIAuthentication(string key) => new(key);

        /// <summary>
        /// Instantiates an empty Authentication object.
        /// </summary>
        public OpenAIAuthentication() { }

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="apiKey">The API key, required to access the API endpoint.</param>
        public OpenAIAuthentication(string apiKey)
        {
            Info = new OpenAIAuthInfo(apiKey);
            cachedDefault = this;
        }

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="apiKey">The API key, required to access the API endpoint.</param>
        /// <param name="organization">
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </param>
        public OpenAIAuthentication(string apiKey, string organization)
        {
            Info = new OpenAIAuthInfo(apiKey, organization);
            cachedDefault = this;
        }

        public OpenAIAuthentication(string apiKey, string organization, string project)
        {
            Info = new OpenAIAuthInfo(apiKey, organization, project);
            cachedDefault = this;
        }

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="authInfo"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="authInfo"></param>
        public OpenAIAuthentication(OpenAIAuthInfo authInfo)
        {
            Info = authInfo;
            cachedDefault = this;
        }

        /// <summary>
        /// Instantiates a new Authentication object with the given <see cref="configuration"/>.
        /// </summary>
        /// <param name="configuration"><see cref="OpenAIConfiguration"/>.</param>
        public OpenAIAuthentication(OpenAIConfiguration configuration) : this(configuration.ApiKey, configuration.organizationId, configuration.projectId) { }

        /// <inheritdoc />
        public override OpenAIAuthInfo Info { get; }

        private static OpenAIAuthentication cachedDefault;

        /// <summary>
        /// The default authentication to use when no other auth is specified.
        /// This can be set manually, or automatically loaded via environment variables or a config file.
        /// <seealso cref="LoadFromEnvironment"/><seealso cref="LoadFromDirectory"/>
        /// </summary>
        public static OpenAIAuthentication Default
        {
            get => cachedDefault ??= new OpenAIAuthentication().LoadDefault();
            internal set => cachedDefault = value;
        }

        /// <inheritdoc />
        public override OpenAIAuthentication LoadFromAsset(OpenAIConfiguration configuration = null)
        {
            if (configuration == null)
            {
                Debug.LogWarning($"You can speed this up by passing a {nameof(OpenAIConfiguration)} to the {nameof(OpenAIAuthentication)}.ctr");
                configuration = Resources.LoadAll<OpenAIConfiguration>(string.Empty).FirstOrDefault(o => o != null);
            }

            return configuration != null ? new OpenAIAuthentication(configuration) : null;
        }

        /// <inheritdoc />
        public override OpenAIAuthentication LoadFromEnvironment()
        {
            var apiKey = Environment.GetEnvironmentVariable(OPENAI_KEY);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = Environment.GetEnvironmentVariable(OPENAI_API_KEY);
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = Environment.GetEnvironmentVariable(OPENAI_SECRET_KEY);
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = Environment.GetEnvironmentVariable(TEST_OPENAI_SECRET_KEY);
            }

            var organizationId = Environment.GetEnvironmentVariable(OPEN_AI_ORGANIZATION_ID);

            if (string.IsNullOrWhiteSpace(organizationId))
            {
                organizationId = Environment.GetEnvironmentVariable(OPENAI_ORGANIZATION_ID);
            }

            if (string.IsNullOrWhiteSpace(organizationId))
            {
                organizationId = Environment.GetEnvironmentVariable(ORGANIZATION);
            }

            return string.IsNullOrEmpty(apiKey) ? null : new OpenAIAuthentication(apiKey, organizationId);
        }

        /// <inheritdoc />
        /// ReSharper disable once OptionalParameterHierarchyMismatch
        public override OpenAIAuthentication LoadFromDirectory(string directory = null, string filename = CONFIG_FILE, bool searchUp = true)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Environment.CurrentDirectory;
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = CONFIG_FILE;
            }

            OpenAIAuthInfo tempAuth = null;

            var currentDirectory = new DirectoryInfo(directory);

            while (tempAuth == null && currentDirectory?.Parent != null)
            {
                var filePath = Path.Combine(currentDirectory.FullName, filename);

                if (File.Exists(filePath))
                {
                    try
                    {
                        tempAuth = JsonUtility.FromJson<OpenAIAuthInfo>(File.ReadAllText(filePath));
                        break;
                    }
                    catch (Exception)
                    {
                        // try to parse the old way for backwards support.
                    }

                    var lines = File.ReadAllLines(filePath);
                    string apiKey = null;
                    string organization = null;

                    foreach (var line in lines)
                    {
                        var parts = line.Split('=', ':');

                        for (var i = 0; i < parts.Length - 1; i++)
                        {
                            var part = parts[i];
                            var nextPart = parts[i + 1];

                            switch (part)
                            {
                                case OPENAI_KEY:
                                case OPENAI_API_KEY:
                                case OPENAI_SECRET_KEY:
                                case TEST_OPENAI_SECRET_KEY:
                                    apiKey = nextPart.Trim();
                                    break;
                                case ORGANIZATION:
                                case OPEN_AI_ORGANIZATION_ID:
                                case OPENAI_ORGANIZATION_ID:
                                    organization = nextPart.Trim();
                                    break;
                            }
                        }
                    }

                    tempAuth = new OpenAIAuthInfo(apiKey, organization);
                }

                if (searchUp)
                {
                    currentDirectory = currentDirectory.Parent;
                }
                else
                {
                    break;
                }
            }

            return string.IsNullOrEmpty(tempAuth?.ApiKey) ? null : new OpenAIAuthentication(tempAuth);
        }
    }
}
