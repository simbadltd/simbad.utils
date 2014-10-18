using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Simbad.Utils.Utils
{
    public static class CloneUtils
    {
        public static T Clone<T>(this T source)
        {
            return Clone(source, Enumerable.Empty<Type>());
        }

        public static T Clone<T>(this T source, IEnumerable<Type> knownTypes)
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(stream);
            }
        }

        public static string SerializeToXml<T>(this T dataToSerialize)
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        public static T DeserializeFromXml<T>(this string xmlText)
        {
            using (var stringReader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
