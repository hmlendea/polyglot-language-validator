using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotTester.Models.Polyglot
{
    public sealed class PolyglotDeclesionCollection
    {
        [XmlElement("declensionNode")]
        public List<PolyglotDeclesionNode> Nodes;

        [XmlElement("decGenRule")]
        public List<PolyglotDeclesionGenerationRule> DeclesionGenerationRules;
    }
}