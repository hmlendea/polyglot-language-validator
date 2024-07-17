using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotLanguageValidator.Models.Polyglot
{
    public sealed class PolyglotPartsOfSpeech
    {
        [XmlElement("class")]
        public List<PolyglotClass> Classes;
    }
}