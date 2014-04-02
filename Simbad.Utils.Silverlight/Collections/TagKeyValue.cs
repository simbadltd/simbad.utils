namespace Simbad.Utils.Collections
{
    public class TagKeyValue
    {
        public string Tag;
        public string Key;
        public string Value;

        public TagKeyValue(string tag, string key, string value)
        {
            Tag = tag;
            Key = key;
            Value = value;
        }
    }
}