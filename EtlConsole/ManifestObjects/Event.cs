using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    public class Event
    {
        [XmlAttribute("symbol")]
        public string Symbol { get; set; }

        [XmlAttribute("value")]
        public int EventId { get; set; }

        [XmlAttribute("version")]
        public int Verion { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("task")]
        public string Task { get; set; }

        [XmlAttribute("opcode")]
        public string Opcode { get; set; }

        [XmlAttribute("template")]
        public string Template { get; set; }
    }
}
