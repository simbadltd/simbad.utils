using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Web;

namespace Simbad.Utils.Extenders
{
    public static class WebOperationContextExtender
    {
        public static bool TrySetResponseTypeByFilename(this WebOperationContext webContext, string filename)
        {
            try
            {
                SetResponseTypeByFilename(webContext, filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetResponseTypeByFilename(this WebOperationContext webContext, string filename)
        {
            var s = Path.GetExtension(filename);
            if (s == null)
            {
                throw (new NotSupportedException());
            }

            var extension = s.ToLower();
            if (!ContentTypes.Value.ContainsKey(extension))
            {
                throw  new NotSupportedException(string.Format("Can not find content-type for this extension ('{0}').", extension));
            }
            
            webContext.OutgoingResponse.ContentType = ContentTypes.Value[extension];
        }

        #region Auxiliary

        private static readonly Lazy<IDictionary<string, string>> ContentTypes = new Lazy<IDictionary<string, string>>(GetContentTypes, true);

        private static IDictionary<string, string> GetContentTypes()
        {
            return new Dictionary<string, string>
                       {
                           {".pdf", "application/pdf"},
                           {".bin", "application/octet-stream"},
                           {".ai", "application/postscript"},
                           {".eps", "application/postscript"},
                           {".ps", "application/postscript"},
                           {".rtf", "application/rtf"},
                           {".zip", "application/zip"},
                           {".xap", "application/x-silverlight-2"},

                           {".gif", "image/gif"},
                           {".jpg", "image/jpeg"},
                           {".jpe", "image/jpeg"},
                           {".jpeg", "image/jpeg"},
                           {".tif", "image/tiff"},
                           {".tiff", "image/tiff"},
                           {".png", "image/png"},

                           {".html", "text/html"},
                           {".htm", "text/html"},
                           {".js", "text/html"},
                           {".txt", "text/plain"},
                           {".css", "text/css"},
                           {".csv", "text/csv"},
                           {".xml", "text/xml"},

                       };
        }

        #endregion
    }
}