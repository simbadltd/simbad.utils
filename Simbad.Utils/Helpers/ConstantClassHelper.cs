using System;
using System.Linq;
using System.Reflection;
using Simbad.Utils.Attributes;
using Simbad.Utils.LookupCore;

namespace Simbad.Utils.Helpers
{
    public static class ConstantClassHelper
    {
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