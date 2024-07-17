using System.Xml.Serialization;

namespace PolyglotLanguageValidator.Models.Polyglot
{
    public sealed class PolyglotDeclesionGenerationRuleTransformation
    {
        [XmlElement("decGenTransRegex")]
        public string Pattern { get; set; }

        [XmlElement("decGenTransReplace")]
        public string Replacement { get; set; }
    }
}