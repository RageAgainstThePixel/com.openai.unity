﻿// Licensed under the MIT License. See LICENSE in the project root for license information.

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
    public sealed class OpenAIAuthentication : AbstractAuthentication<OpenAIAuthentication, OpenAIAuthInfo>
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
        public static implicit operator OpenAIAuthentication(string key) => new OpenAIAuthentication(key);

        /// <summary>
        /// Instantiates a new Authentication object that will load the default config.
        /// </summary>
        public OpenAIAuthentication()
        {
            if (cachedDefault != null) { return; }

            cachedDefault = (LoadFromAsset<OpenAIConfiguration>() ??
                             LoadFromDirectory()) ??
                             LoadFromDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)) ??
                             LoadFromEnvironment();
            Info = cachedDefault?.Info;
        }

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="apiKey">The API key, required to access the API endpoint.</param>
        public OpenAIAuthentication(string apiKey) => Info = new OpenAIAuthInfo(apiKey);

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="apiKey">The API key, required to access the API endpoint.</param>
        /// <param name="organization">
        /// For users who belong to multiple organizations, you can pass a header to specify which organization is used for an API request.
        /// Usage from these API requests will count against the specified organization's subscription quota.
        /// </param>
        public OpenAIAuthentication(string apiKey, string organization) => Info = new OpenAIAuthInfo(apiKey, organization);

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="authInfo"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="authInfo"></param>
        public OpenAIAuthentication(OpenAIAuthInfo authInfo) => Info = authInfo;

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
            get => cachedDefault ?? new OpenAIAuthentication();
            internal set => cachedDefault = value;
        }

        /// <inheritdoc />
        public override OpenAIAuthentication LoadFromAsset<T>()
            => Resources.LoadAll<T>(string.Empty)
                .Where(asset => asset != null)
                .Where(asset => asset is OpenAIConfiguration config &&
                                !string.IsNullOrWhiteSpace(config.ApiKey))
                .Select(asset => asset is OpenAIConfiguration config
                    ? new OpenAIAuthentication(config.ApiKey, config.OrganizationId)
                    : null)
                .FirstOrDefault();

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
            directory ??= Environment.CurrentDirectory;

            OpenAIAuthInfo tempAuth = null;

            var currentDirectory = new DirectoryInfo(directory);

            while (tempAuth == null && currentDirectory.Parent != null)
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

            if (tempAuth == null ||
                string.IsNullOrEmpty(tempAuth.ApiKey))
            {
                return null;
            }

            return new OpenAIAuthentication(tempAuth);
        }
    }
}
