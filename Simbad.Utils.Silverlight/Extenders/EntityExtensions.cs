namespace Simbad.Utils.Extenders
{
    public static class NullExtender
    {
        public static bool Exists(this object obj)
        {
            return obj != null;
        }

        public static bool NotExists(this object obj)
        {
            return obj == null;
        }
    }
}