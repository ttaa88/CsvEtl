using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    public class Localization
    {
        [XmlElement("resources", typeof(Resources))]
        public Resources Resources { get; set; }
    }

    public class Resources
    {
        [XmlArray("stringTable")]
        [XmlArrayItem("string", typeof(CustomString))]
        public CustomString[] Strings { get; set; }
    }

    public class CustomString
    {
        [XmlElement("id", typeof(string))]
        public string Id { get; set; }

        [XmlElement("value", typeof(string))]
        public string Value { get; set; }
    }
}
