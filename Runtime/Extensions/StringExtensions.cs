using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpenAI.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Generates a <see cref="Guid"/> based on the string.
        /// </summary>
        /// <param name="string">The string to generate the <see cref="Guid"/>.</param>
        /// <returns>A new <see cref="Guid"/> that represents the string.</returns>
        public static Guid GenerateGuid(this string @string)
        {
            using MD5 md5 = MD5.Create();
            return new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(@string)));
        }

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

        public static string GetPathSafeString(this string path)
            => Path.GetInvalidFileNameChars().Aggregate(path, (current, c) => current.Replace(c, '_'));
    }
}
