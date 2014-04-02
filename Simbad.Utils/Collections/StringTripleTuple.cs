namespace Simbad.Utils.Collections
{
    public class StringTripleTuple: TripleTuple<string, string, string>
    {
        public StringTripleTuple(string item1, string item2, string item3)
            : base(item1, item2, item3)
        {
        }

        public StringTripleTuple()
        {
        }
    }
}