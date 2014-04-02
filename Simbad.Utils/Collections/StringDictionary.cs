using System.Collections.Generic;
using System.Text;

namespace Simbad.Utils.Collections
{
    public class StringDictionary: Dictionary<string, string>
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

        public StringDictionary()
            : this(string.Empty,
                   DEFAULT_ITEMS_SEPARATOR,
                   DEFAULT_VALUES_SEPARATOR)
        {
        }

        public StringDictionary(char itemsSeparator, char valuesSeparator)
            : this(string.Empty,
                   itemsSeparator,
                   valuesSeparator)
        {
        }

        public StringDictionary(string data)
            : this(data,
                   DEFAULT_ITEMS_SEPARATOR,
                   DEFAULT_VALUES_SEPARATOR)
        {
        }

        public StringDictionary(string data, char itemsSeparator, char valuesSeparator)
        {
            _itemsSeparator = itemsSeparator;
            _valuesSeparator = valuesSeparator;

            if (!string.IsNullOrEmpty(data))
            {
                Deserialize(data);
            }
        }

        #endregion

        public new string this[string key]
        {
            get
            {
                return base[key];
            }

            set
            {
                var normalizedKey = RemoveInvalidChars(key);

                if (ContainsKey(normalizedKey))
                {
                    return;
                }

                var normalizedValue = RemoveInvalidChars(value);
                Add(normalizedKey, normalizedValue);
            }
        }

        public new void Add(string key, string value)
        {
            var normalizedKey = RemoveInvalidChars(key);
            var normalizedValue = RemoveInvalidChars(value);
            base.Add(normalizedKey, normalizedValue);
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
                if (values.Length != 2)
                {
                    continue;
                }

                this[values[0]] = values[1];
            }
        }

        public string Serialize()
        {
            var sb = new StringBuilder();

            foreach (var keyValue in this)
            {
                sb.Append(keyValue.Key);
                sb.Append(_valuesSeparator);
                sb.Append(keyValue.Value);
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