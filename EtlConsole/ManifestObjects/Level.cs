using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    public class Level
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("symbol")]
        public string Symbol { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }
    }
}
