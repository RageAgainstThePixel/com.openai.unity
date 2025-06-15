// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Models;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

namespace OpenAI.Images
{
    [Preserve]
    public sealed class ImageVariationRequest : AbstractBaseImageRequest, IDisposable
    {
        [Preserve]
        public ImageVariationRequest(
            string imagePath,
            int? numberOfResults = null,
            string size = null,
            string user = null,
            ImageResponseFormat responseFormat = 0,
            Model model = null)
            : this(
                image: (Path.GetFileName(imagePath), File.OpenRead(imagePath)),
                numberOfResults: numberOfResults,
                size: size,
                user: user,
                responseFormat: responseFormat, model: model)
        {
        }

        [Preserve]
        public ImageVariationRequest(
            Texture2D texture,
            int? numberOfResults = null,
            string size = null,
            string user = null,
            ImageResponseFormat responseFormat = 0,
            Model model = null)
            : this(
                image: (!string.IsNullOrWhiteSpace(texture.name) ? $"{texture.name}.png" : null, new MemoryStream(texture.EncodeToPNG())),
                numberOfResults: numberOfResults,
                size: size,
                user: user,
                responseFormat: responseFormat,
                model: model)
        {
        }

        [Preserve]
        public ImageVariationRequest(
            (string, Stream) image,
            int? numberOfResults = null,
            string size = null,
            string user = null,
            ImageResponseFormat responseFormat = 0,
            Model model = null)
            : base(model, numberOfResults, size, responseFormat, user)
        {
            var (imageName, imageStream) = image;
            Image = imageStream ?? throw new ArgumentNullException(nameof(imageStream));
            ImageName = string.IsNullOrWhiteSpace(imageName) ? "image.png" : imageName;
        }

        #region Obsolete .ctors

        [Obsolete("Use new .ctor overload")]
        public ImageVariationRequest(
            string imagePath,
            int numberOfResults = 1,
            ImageSize size = ImageSize.Large,
            string user = null,
            ImageResponseFormat responseFormat = ImageResponseFormat.Url,
            Model model = null)
            : this(File.OpenRead(imagePath), Path.GetFileName(imagePath), numberOfResults, size, user, responseFormat, model)
        {
        }

        [Obsolete("Use new .ctor overload")]
        public ImageVariationRequest(
            Texture2D texture,
            int numberOfResults,
            ImageSize size = ImageSize.Large,
            string user = null,
            ImageResponseFormat responseFormat = ImageResponseFormat.Url,
            Model model = null)
            : this(
                new MemoryStream(texture.EncodeToPNG()),
                !string.IsNullOrWhiteSpace(texture.name) ? $"{texture.name}.png" : null,
                numberOfResults,
                size,
                user,
                responseFormat,
                model)
        {
        }

        [Obsolete("Use new .ctor overload")]
        public ImageVariationRequest(
            Stream image,
            string imageName,
            int numberOfResults,
            ImageSize size = ImageSize.Large,
            string user = null,
            ImageResponseFormat responseFormat = ImageResponseFormat.Url,
            Model model = null)
            : base(model, numberOfResults, size, responseFormat, user)
        {
            Image = image;

            if (string.IsNullOrWhiteSpace(imageName))
            {
                const string defaultImageName = "image.png";
                imageName = defaultImageName;
            }

            ImageName = imageName;

            if (numberOfResults is > 10 or < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfResults), "The number of results must be between 1 and 10");
            }
        }

        #endregion Obsolete .ctors

        ~ImageVariationRequest() => Dispose(false);

        /// <summary>
        /// The image to use as the basis for the variation(s). Must be a valid PNG file, less than 4MB, and square.
        /// </summary>
        [Preserve]
        public Stream Image { get; }

        [Preserve]
        public string ImageName { get; }

        [Preserve]
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Image?.Dispose();
            }
        }

        [Preserve]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
