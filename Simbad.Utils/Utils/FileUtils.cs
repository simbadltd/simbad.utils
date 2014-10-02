using System.IO;

namespace Simbad.Utils.Utils
{
    public static class FileUtils
    {
        public static bool TryDeleteFile(string path)
        {
            try
            {
                File.Delete(path);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}