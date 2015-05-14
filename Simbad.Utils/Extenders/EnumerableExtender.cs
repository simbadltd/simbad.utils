using System.Collections.Generic;
using System.Linq;

namespace Simbad.Utils.Extenders
{
    public static class EnumerableExtender
    {
        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return IsEmpty(collection) == false;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Any() == false;
        }
    }
}