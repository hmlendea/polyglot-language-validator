using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotLanguageValidator.Models.Polyglot
{
    public sealed class PolyglotLexicon
    {
        [XmlElement("word")]
        public List<PolyglotWord> Words;
    }
}