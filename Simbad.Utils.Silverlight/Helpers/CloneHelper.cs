using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Simbad.Utils.Silverlight.Helpers
{
    public static class CloneHelper
    {
        public static T Clone<T>(this T source)
        {
            var ser = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                ser.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)ser.ReadObject(stream);
            }
        }

        public static string SerializeToXml<T>(this T dataToSerialize)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, dataToSerialize);
            return stringwriter.ToString();
        }

        public static T DeserializeFromXml<T>(this string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}