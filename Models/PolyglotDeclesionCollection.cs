using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotTester.Models
{
    public sealed class PolyglotDeclesionCollection
    {
        [XmlElement("declensionNode")]
        public List<PolyglotDeclesionNode> Nodes;
    }
}