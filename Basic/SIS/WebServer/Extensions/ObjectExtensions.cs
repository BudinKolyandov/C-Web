using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Xml.Serialization;

namespace SIS.MvcFramework.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToXml(this object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(obj.GetType());
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                serializer.Serialize(stringWriter, obj, namespaces);
                return stringWriter.ToString();
            }
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
