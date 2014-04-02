using System.Collections.Generic;

namespace Simbad.Utils.Collections
{
    public class StringList: List<string>
    {
        public const char DEFAULT_ITEMS_SEPARATOR = ';';

        public const char DEFAULT_ALTERNATE_ITEMS_SEPARATOR = '|';

        private readonly char _separator;

        #region ctor

        public StringList()
            : this(string.Empty, DEFAULT_ITEMS_SEPARATOR)
        {
        }

        public StringList(char itemsSeparator)
            : this(string.Empty, itemsSeparator)
        {
        }

        public StringList(string data)
            : this(data, DEFAULT_ITEMS_SEPARATOR)
        {
        }

        public StringList(string data, char separator)
        {
            _separator = separator;

            if (!string.IsNullOrEmpty(data))
            {
                Deserialize(data);
            }
        }

        #endregion

        public new void Add(string item)
        {
            var normalizedItem = RemoveInvalidChars(item);
            base.Add(normalizedItem);
        }

        private string RemoveInvalidChars(string s)
        {
            return
                s.Replace(_separator.ToString(), string.Empty);
        }

        public void Deserialize(string data)
        {
            Clear();

            if (string.IsNullOrEmpty(data))
            {
                return;
            }

            var rows = data.Split(_separator);

            foreach (var row in rows)
            {
                Add(row);
            }
        }

        public string Serialize()
        {
            return string.Join(_separator.ToString(), ToArray());
        }
    }
}