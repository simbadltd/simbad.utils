using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Simbad.Utils.Utils
{
    public static class WebDownloader
    {
        public static bool TryDownloadRemoteImageFile(string remoteFile, string localFile)
        {
            try
            {
                DownloadRemoteImageFile(remoteFile, localFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DownloadRemoteImageFile(string remoteFile, string localFile)
        {
            DownloadFile(remoteFile, localFile, s => s.StartsWith("image", StringComparison.OrdinalIgnoreCase));
        }

        public static void DownloadFile(string remoteFile, string localFile, Func<string, bool> contentTypeFilter = null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(localFile));

            using (var outputStream = File.OpenWrite(localFile))
            {
                DownloadFileInternal(remoteFile, outputStream, contentTypeFilter);
            }
        }

        private static void DownloadFileInternal(string remoteFile, Stream outputStream, Func<string, bool> contentTypeFilter)
        {
            var request = (HttpWebRequest)WebRequest.Create(remoteFile);
            var response = (HttpWebResponse)request.GetResponse();
            var isResponseStatusWithoutError = IsResponseStatusWithoutError(response);
            var isContentTypeValid = contentTypeFilter == null || contentTypeFilter(response.ContentType);

            if (isResponseStatusWithoutError && isContentTypeValid)
            {
                using (var inputStream = response.GetResponseStream())
                {
                    Debug.Assert(inputStream != null, "inputStream cannot be null");

                    inputStream.CopyTo(outputStream);
                }
            }
        }

        private static bool IsResponseStatusWithoutError(HttpWebResponse response)
        {
            var result = response.StatusCode == HttpStatusCode.OK ||
                         response.StatusCode == HttpStatusCode.Moved ||
                         response.StatusCode == HttpStatusCode.Redirect;

            return result;
        }
    }
}
