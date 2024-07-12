using System.Xml.Serialization;

namespace PolyglotTester.Models
{
    public sealed class PolyglotDeclesionNode
    {
        [XmlElement("declensionId")]
        public int Id { get; set; }

        [XmlElement("declensionText")]
        public string Name { get; set; }

        [XmlElement("declensionRelatedId")]
        public int PartOfSpeechId { get; set; }
    }
}