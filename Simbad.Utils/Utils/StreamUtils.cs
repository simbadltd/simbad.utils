using System.IO;

namespace Simbad.Utils.Utils
{
    public static class StreamUtils
    {
        public static string AsString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}