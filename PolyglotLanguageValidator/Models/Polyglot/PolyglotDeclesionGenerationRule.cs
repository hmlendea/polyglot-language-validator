using System.Collections.Generic;
using System.Xml.Serialization;

namespace PolyglotLanguageValidator.Models.Polyglot
{
    public sealed class PolyglotDeclesionGenerationRule
    {
        [XmlElement("decGenRuleComb")]
        public string RuleCombination { get; set; }

        [XmlElement("decGenRuleRegex")]
        public string RegularExpression { get; set; }

        [XmlElement("decGenRuleName")]
        public string Name { get; set; }

        [XmlElement("decGenRuleTypeId")]
        public int PartOfSpeechId { get; set; }

        [XmlElement("decGenRuleIndex")]
        public int Index { get; set; }

        [XmlElement("decGenTrans")]
        public List<PolyglotDeclesionGenerationRuleTransformation> Transformations { get; set; }
    }
}