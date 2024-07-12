using System.Xml.Serialization;

namespace PolyglotTester.Models
{
    [XmlRoot("dictionary")]
    public sealed class PolyglotDictionary
    {
        [XmlElement("partsOfSpeech")]
        public PolyglotPartsOfSpeech PartsOfSpeech { get; set; }

        [XmlElement("lexicon")]
        public PolyglotLexicon Lexicon { get; set; }

        [XmlElement("declensionCollection")]
        public PolyglotDeclesionCollection DeclesionCollection { get; set; }
    }
}