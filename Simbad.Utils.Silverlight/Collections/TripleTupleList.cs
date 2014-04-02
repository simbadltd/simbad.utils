using System.Collections.Generic;
using System.Text;

namespace Simbad.Utils.Collections
{
    public class TripleTupleList : List<StringTripleTuple>
    {
        #region Constants

        public const char DEFAULT_ITEMS_SEPARATOR = ';';

        public const char DEFAULT_VALUES_SEPARATOR = '=';

        public const char DEFAULT_ALTERNATE_ITEMS_SEPARATOR = '|';

        public const char DEFAULT_ALTERNATE_VALUES_SEPARATOR = '~';

        #endregion

        #region Variables

        private readonly char _itemsSeparator;

        private readonly char _valuesSeparator;

        #endregion

        #region ctor

        public TripleTupleList()
            : this(
                string.Empty,
                DEFAULT_ITEMS_SEPARATOR,
                DEFAULT_VALUES_SEPARATOR)
        {
        }

        public TripleTupleList(char itemsSeparator, char valuesSeparator)
            : this(string.Empty,
                   itemsSeparator,
                   valuesSeparator)
        {
        }

        public TripleTupleList(string data)
            : this(data,
                   DEFAULT_ITEMS_SEPARATOR,
                   DEFAULT_VALUES_SEPARATOR)
        {
        }

        public TripleTupleList(string data, char itemsSeparator, char valuesSeparator)
        {
            _itemsSeparator = itemsSeparator;
            _valuesSeparator = valuesSeparator;

            if (!string.IsNullOrEmpty(data))
            {
                Deserialize(data);
            }
        }

        #endregion

        public void Add(string item1, string item2, string item3)
        {
            Add(new StringTripleTuple(item1, item2, item3));
        }

        public new void Add(StringTripleTuple item)
        {
            if (item == null)
            {
                return;
            }

            item.Item1 = RemoveInvalidChars(item.Item1);
            item.Item2 = RemoveInvalidChars(item.Item2);
            item.Item3 = RemoveInvalidChars(item.Item3);

            base.Add(item);
        }

        private string RemoveInvalidChars(string s)
        {
            return
                s.Replace(_itemsSeparator.ToString(), string.Empty)
                 .Replace(_valuesSeparator.ToString(), string.Empty);
        }

        public void Deserialize(string data)
        {
            Clear();

            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var items = data.Split(_itemsSeparator);

            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                var values = item.Split(_valuesSeparator);
                if (values.Length != 3)
                {
                    continue;
                }

                Add(values[0],values[1],values[2]);
            }
        }

        public string Serialize()
        {
            var sb = new StringBuilder();

            foreach (var keyValue in this)
            {
                sb.Append(keyValue.Item1);
                sb.Append(_valuesSeparator);
                sb.Append(keyValue.Item2);
                sb.Append(_valuesSeparator);
                sb.Append(keyValue.Item3);
                sb.Append(_itemsSeparator);
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}