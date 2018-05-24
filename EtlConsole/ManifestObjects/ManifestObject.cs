using System.Xml.Serialization;

namespace ConsoleApp.Xml
{
    [XmlRoot(ElementName="instrumentationManifest", 
             Namespace="http://schemas.microsoft.com/win/2004/08/events")]
    public class InstrumentationManifest
    {
        [XmlElement("instrumentation", typeof(Instrumentation))]
        public Instrumentation Instrumentation { get; set; }

        [XmlElement("localization", typeof(Localization))]
        public Localization Localization { get; set; }
    }

    public class Instrumentation
    {
        [XmlElement("events", typeof(OuterEvent))]
        public OuterEvent OuterEvent { get; set; }
    }

    public class OuterEvent
    {
        [XmlElement("provider", typeof(Provider))]
        public Provider Provider { get; set; }
    }

    public class Provider
    {
        [XmlArray("events")]
        [XmlArrayItem("event", typeof(Event))]
        public Event[] Events { get; set; }

        [XmlArray("levels")]
        [XmlArrayItem("level", typeof(Level))]
        public Level[] Levels { get; set; }

        [XmlArray("tasks")]
        [XmlArrayItem("task", typeof(Task))]
        public Task[] Tasks { get; set; }

        [XmlArray("maps")]
        [XmlArrayItem("valueMap", typeof(ValueMap))]
        public ValueMap[] ValueMaps { get; set; }

        [XmlArray("templates")]
        [XmlArrayItem("template", typeof(Template))]
        public Template[] TemplateCollection { get; set; }
    }    
}
        
