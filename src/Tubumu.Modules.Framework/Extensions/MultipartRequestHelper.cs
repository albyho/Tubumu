using System;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// MultipartRequestHelper
    /// </summary>
    public static class MultipartRequestHelper
    {
        /// <summary>
        /// GetBoundary
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="lengthLimit"></param>
        /// <returns></returns>
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).ToString();
            if (boundary == StringSegment.Empty)
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        /// <summary>
        /// IsMultipartContentType
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// HasFormDataContentDisposition
        /// </summary>
        /// <param name="contentDisposition"></param>
        /// <returns></returns>
        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && contentDisposition.FileName.ToString() == String.Empty
                   && contentDisposition.FileNameStar.ToString() == String.Empty;
        }

        /// <summary>
        /// HasFileContentDisposition
        /// </summary>
        /// <param name="contentDisposition"></param>
        /// <returns></returns>
        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (contentDisposition.FileName.ToString() != String.Empty || contentDisposition.FileNameStar.ToString() != String.Empty);
        }
    }
}
