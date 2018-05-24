using System;
using System.IO;
using System.Xml.Serialization;
using ConsoleApp.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace EtlConsoleTest
{
    [TestClass]
    public class XmlDeserializeTest
    {
        [TestMethod]
        public void TestDeserialization()
        {
            InstrumentationManifest obj = null;
            string path = @".\TestXml\DispatchEventProvider.man";
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InstrumentationManifest));
                try
                {
                    obj = (InstrumentationManifest)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException e)
                {
                }
            }

            obj.Localization.Resources.Strings.Length.ShouldBe(28);
            obj.Instrumentation.OuterEvent.Provider.Events.Length.ShouldBe(45);
            obj.Instrumentation.OuterEvent.Provider.Levels.Length.ShouldBe(1);
            obj.Instrumentation.OuterEvent.Provider.Tasks.Length.ShouldBe(7);
            obj.Instrumentation.OuterEvent.Provider.ValueMaps.Length.ShouldBe(4);
            obj.Instrumentation.OuterEvent.Provider.ValueMaps[0].Maps.Length.ShouldBe(6);
            obj.Instrumentation.OuterEvent.Provider.TemplateCollection[0].DataCollection.Length.ShouldBe(7);
        }
    }
}
