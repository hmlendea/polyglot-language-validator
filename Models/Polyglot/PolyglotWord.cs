using System.Xml.Serialization;

namespace PolyglotTester.Models.Polyglot
{
    public sealed class PolyglotWord
    {
        [XmlElement("wordId")]
        public int WordId { get; set; }

        [XmlElement("localWord")]
        public string LocalWord { get; set; }

        [XmlElement("conWord")]
        public string ConstructedWord { get; set; }

        [XmlElement("wordTypeId")]
        public int WordTypeId { get; set; }
    }
}