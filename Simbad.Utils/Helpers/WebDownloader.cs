using System;
using System.IO;
using System.Net;

namespace Simbad.Utils.Helpers
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
            var request = (HttpWebRequest)WebRequest.Create(remoteFile);
            var response = (HttpWebResponse)request.GetResponse();

            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localFile));

                using (var inputStream = response.GetResponseStream())
                using (var outputStream = File.OpenWrite(localFile))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }
    }
}
