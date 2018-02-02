using System.IO;
using System.Xml.Serialization;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    ///Xml工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 18:30:27")]
    [ClassPurposeDesc("Xml工具", "Xml工具")]
#endif
    public static class XmlUitlity
    {
        public static void SaveToXml<T>(string filePath, T sourceObj, string xmlRootName = null)
        {
            if (string.IsNullOrEmpty(filePath) || sourceObj == null) return;

            var type = typeof(T);
            xmlRootName = xmlRootName ?? typeof(T).Name;

            using (var writer = new StreamWriter(filePath))
            {
                var xmlSerializer = string.IsNullOrEmpty(xmlRootName) ?
                    new XmlSerializer(type) :
                    new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                xmlSerializer.Serialize(writer, sourceObj);
            }
        }

        public static T LoadFromXml<T>(string filePath) where T : class
        {
            var type = typeof(T);
            T result;

            if (!File.Exists(filePath)) return default(T);

            using (var reader = new StreamReader(filePath))
            {
                var xmlSerializer = new XmlSerializer(type);
                result = xmlSerializer.Deserialize(reader).As<T>();
            }

            return result;
        }
    }
}
