// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OpenAI
{
    /// <summary>
    /// Represents authentication to the OpenAPI API endpoint
    /// </summary>
    public class OpenAIAuthentication
    {
        /// <summary>
        /// The API key, required to access the API endpoint.
        /// </summary>
        public string ApiKey { get; }

        /// <summary>
        /// Allows implicit casting from a string, so that a simple string API key can be provided in place of an instance of <see cref="OpenAIAuthentication"/>
        /// </summary>
        /// <param name="key">The API key to convert into a <see cref="OpenAIAuthentication"/>.</param>
        public static implicit operator OpenAIAuthentication(string key) => new OpenAIAuthentication(key);

        /// <summary>
        /// Instantiates a new Authentication object with the given <paramref name="apiKey"/>, which may be <see langword="null"/>.
        /// </summary>
        /// <param name="apiKey">The API key, required to access the API endpoint.</param>
        public OpenAIAuthentication(string apiKey) => ApiKey = apiKey;

        private static OpenAIAuthentication cachedDefault = null;

        /// <summary>
        /// The default authentication to use when no other auth is specified.  This can be set manually, or automatically loaded via environment variables or a config file.  <seealso cref="LoadFromEnv"/><seealso cref="LoadFromDirectory"/>
        /// </summary>
        public static OpenAIAuthentication Default
        {
            get
            {
                if (cachedDefault != null)
                {
                    return cachedDefault;
                }

                var auth = (LoadFromAsset() ??
                            LoadFromEnv() ??
                            LoadFromDirectory()) ??
                            LoadFromDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                cachedDefault = auth;
                return auth;
            }
            set => cachedDefault = value;
        }

        private static OpenAIAuthentication LoadFromAsset()
        {
            var assets = UnityEngine.Object.FindObjectsOfType<OpenAIConfigurationSettings>().ToList();
            assets.AddRange(Resources.FindObjectsOfTypeAll<OpenAIConfigurationSettings>().ToList());
            return (from asset in assets where !string.IsNullOrWhiteSpace(asset.ApiKey) &&
                                               !string.IsNullOrWhiteSpace(asset.ApiKey)
                select new OpenAIAuthentication(asset.ApiKey)).FirstOrDefault();
        }


        /// <summary>
        /// Attempts to load api keys from environment variables, as "OPENAI_KEY" (or "OPENAI_SECRET_KEY", for backwards compatibility)
        /// </summary>
        /// <returns>Returns the loaded <see cref="OpenAIAuthentication"/> any api keys were found, or <see langword="null"/> if there were no matching environment vars.</returns>
        public static OpenAIAuthentication LoadFromEnv()
        {
            var key = Environment.GetEnvironmentVariable("OPENAI_KEY");

            if (string.IsNullOrWhiteSpace(key))
            {
                key = Environment.GetEnvironmentVariable("OPENAI_SECRET_KEY");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                key = Environment.GetEnvironmentVariable("TEST_OPENAI_SECRET_KEY");
            }

            return string.IsNullOrEmpty(key) ? null : new OpenAIAuthentication(key);
        }

        /// <summary>
        /// Attempts to load api keys from a configuration file, by default ".openai" in the current directory, optionally traversing up the directory tree
        /// </summary>
        /// <param name="directory">The directory to look in, or <see langword="null"/> for the current directory</param>
        /// <param name="filename">The filename of the config file</param>
        /// <param name="searchUp">Whether to recursively traverse up the directory tree if the <paramref name="filename"/> is not found in the <paramref name="directory"/></param>
        /// <returns>Returns the loaded <see cref="OpenAIAuthentication"/> any api keys were found, or <see langword="null"/> if it was not successful in finding a config (or if the config file didn't contain correctly formatted API keys)</returns>
        public static OpenAIAuthentication LoadFromDirectory(string directory = null, string filename = ".openai", bool searchUp = true)
        {
            directory = directory ?? (directory = Environment.CurrentDirectory);

            string key = null;
            var curDirectory = new DirectoryInfo(directory);

            while (key == null && curDirectory.Parent != null)
            {
                if (File.Exists(Path.Combine(curDirectory.FullName, filename)))
                {
                    var lines = File.ReadAllLines(Path.Combine(curDirectory.FullName, filename));
                    foreach (var l in lines)
                    {
                        var parts = l.Split('=', ':');
                        if (parts.Length == 2)
                        {
                            switch (parts[0].ToUpper())
                            {
                                case "OPENAI_KEY":
                                    key = parts[1].Trim();
                                    break;
                                case "OPENAI_SECRET_KEY":
                                    key = parts[1].Trim();
                                    break;
                            }
                        }
                    }
                }

                if (searchUp)
                {
                    curDirectory = curDirectory.Parent;
                }
                else
                {
                    break;
                }
            }

            return string.IsNullOrEmpty(key) ? null : new OpenAIAuthentication(key);
        }
    }
}
