using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Simbad.Utils.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetDescription(this object value)
        {
            try
            {
                var attribute = value.GetType()
                                    .GetField(value.ToString())
                                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                    .SingleOrDefault() as DescriptionAttribute;

                return attribute.Description;
            }
            catch (Exception)
            {
                return value.ToString();
            }
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException();
            }

            var fields = type.GetFields();

            if (fields == null || !fields.Any())
            {
                return default(T);
            }

            var field =
                fields.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false), (f, a) => new { Field = f, Att = a })
                    .SingleOrDefault(a => ((DescriptionAttribute)a.Att).Description == description);

            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }
    }
}
