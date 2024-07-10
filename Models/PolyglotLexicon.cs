using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotTester.Models
{
    public sealed class PolyglotLexicon
    {
        [XmlElement("word")]
        public List<PolyglotWord> Words;
    }
}