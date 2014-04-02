using System;

namespace Simbad.Utils.Attributes
{
    public class StringValueAttribute : Attribute
    {
        public string Value { get; set; }

        public StringValueAttribute()
            : this(string.Empty)
        {
        }

        public StringValueAttribute(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            var descriptionAttribute = obj as StringValueAttribute;
            if (descriptionAttribute != null)
                return descriptionAttribute.Value == this.Value;
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}