using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    public class ValueMap
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("map", typeof(Map))]
        public Map[] Maps { get; set; }
    }

    public class Map
    {
        [XmlAttribute("value")]
        public int Value { get; set; }

        [XmlAttribute("message")]
        public string Message { get; set; }
    }
}
