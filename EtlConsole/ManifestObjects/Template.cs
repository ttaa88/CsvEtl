using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    public class Template
    {
        [XmlAttribute("tid")]
        public string Tid { get; set; }

        [XmlElement("data", typeof(Data))]
        public Data[] DataCollection { get; set; }
    }

    public class Data
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("inType")]
        public string InType { get; set; }

        [XmlAttribute("outType")]
        public string OutType { get; set; }
    }
}
