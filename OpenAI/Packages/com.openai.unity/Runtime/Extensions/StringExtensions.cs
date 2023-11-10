using System;
using System.IO;

namespace OpenAI.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Create a new directory based on the current string format.
        /// </summary>
        /// <param name="parentDirectory"></param>
        /// <param name="newDirectoryName"></param>
        /// <returns>Full path to the newly created directory.</returns>
        public static string CreateNewDirectory(this string parentDirectory, string newDirectoryName)
        {
            if (string.IsNullOrWhiteSpace(parentDirectory))
            {
                throw new ArgumentNullException(nameof(parentDirectory));
            }

            if (string.IsNullOrWhiteSpace(newDirectoryName))
            {
                throw new ArgumentNullException(nameof(newDirectoryName));
            }

            var directory = Path.Combine(parentDirectory, newDirectoryName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}
