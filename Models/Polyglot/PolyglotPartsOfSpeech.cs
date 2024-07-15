using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotTester.Models.Polyglot
{
    public sealed class PolyglotPartsOfSpeech
    {
        [XmlElement("class")]
        public List<PolyglotClass> Classes;
    }
}