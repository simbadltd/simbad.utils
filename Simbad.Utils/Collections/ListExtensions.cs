using System.Collections.Generic;

namespace Simbad.Utils.Collections
{
    public static class ListExtensions
    {
        public static void MoveUp<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);
            MoveUp(list, index);
        }

        public static void MoveUp<T>(this IList<T> list, int index)
        {
            if (index <= 0)
            {
                return;
            }

            Swap(list, index, index - 1);
        }

        public static void MoveDown<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);
            MoveDown(list, index);
        }

        public static void MoveDown<T>(this IList<T> list, int index)
        {
            if (index >= list.Count - 1)
            {
                return;
            }

            Swap(list, index, index + 1);
        }

        public static void Swap<T>(this IList<T> list, T itemA, T itemB)
        {
            var indexA = list.IndexOf(itemA);
            var indexB = list.IndexOf(itemB);

            Swap(list, indexA, indexB);
        }

        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}
