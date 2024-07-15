using System.Xml.Serialization;

namespace PolyglotTester.Models.Polyglot
{
    public sealed class PolyglotDimensionNode
    {
        [XmlElement("dimensionId")]
        public int Id { get; set; }

        [XmlElement("dimensionName")]
        public string Name { get; set; }
    }
}