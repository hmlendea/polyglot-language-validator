using System.Xml.Serialization;

namespace PolyglotLanguageValidator.Models.Polyglot
{
    public sealed class PolyglotDimensionNode
    {
        [XmlElement("dimensionId")]
        public int Id { get; set; }

        [XmlElement("dimensionName")]
        public string Name { get; set; }
    }
}