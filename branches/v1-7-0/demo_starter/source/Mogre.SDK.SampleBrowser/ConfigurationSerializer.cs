using System.IO;
using System.Xml.Serialization;

namespace Mogre.SDK.SampleBrowser
{
    public class ConfigurationSerializer
    {
        private readonly XmlSerializer _serializer;

        public ConfigurationSerializer()
        {
            _serializer = new XmlSerializer(typeof (SampleBrowser));
        }

        public void Serialize(string filename, Sample[] samples)
        {
            var sampleBrowser = new SampleBrowser {Samples = samples};

            using (var stream = new FileStream(filename, FileMode.Create))
                _serializer.Serialize(stream, sampleBrowser);
        }

        public Sample[] Deserialize(string filename)
        {
            SampleBrowser sampleBrowser;

            try
            {
                using (var stream = new FileStream(filename, FileMode.Open))
                    sampleBrowser = (SampleBrowser) _serializer.Deserialize(stream);
            }
            catch
            {
                return null;
            }

            return sampleBrowser.Samples;
        }

        [XmlRoot("sampleBrowser")]
        public struct SampleBrowser
        {
            [XmlElement("sample")] 
            public Sample[] Samples { get; set; }
        }

        public class Sample
        {
            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("category")]
            public string Category { get; set; }

            [XmlElement("executablePath")]
            public string ExecutablePath { get; set; }

            [XmlElement("previewImagePath")]
            public string PreviewImagePath { get; set; }

            [XmlElement("tutorialLink")]
            public string TutorialLink { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
