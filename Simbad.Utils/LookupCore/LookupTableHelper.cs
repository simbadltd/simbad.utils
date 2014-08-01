using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Simbad.Utils.Attributes;
using Simbad.Utils.Utils;

namespace Simbad.Utils.LookupCore
{
    public static class LookupTableHelper
    {
        public static LookupTable<string, string> EnumToLookupTable(this Type type)
        {
            var toReturn = new LookupTable<string, string>();

            if (!type.IsEnum)
            {
                return toReturn;
            }

            var names = Enum.GetNames(type);

            foreach (var name in names)
            {
                var enumValue = Enum.Parse(type, name);
                toReturn.Add(new LookupRecord<string, string>(enumValue.GetDescription(), ((int)enumValue).ToString(CultureInfo.InvariantCulture)));
            }

            return toReturn;
        }

        public static LookupTable<string, string> ToLookupTable(this Type type)
        {
            var toReturn = new LookupTable<string, string>();

            var constants =
                type.GetFields(BindingFlags.Public |
                               BindingFlags.Static |
                               BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                    .ToList();

            foreach (var constant in constants)
            {
                var attrs = constant.GetCustomAttributes(true);
                var stringValueAttribute = attrs.OfType<StringValueAttribute>().FirstOrDefault();

                if (stringValueAttribute == null)
                {
                    continue;
                }

                var name = stringValueAttribute.Value; 
                var value = constant.GetValue(null).ToString();
                toReturn.Add(new LookupRecord<string, string>(name, value));
            }

            return toReturn;
        }
    }
}