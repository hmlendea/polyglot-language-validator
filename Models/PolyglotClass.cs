using System.Xml.Serialization;

namespace PolyglotTester.Models
{
    public sealed class PolyglotClass
    {
        [XmlElement("classId")]
        public int ClassId { get; set; }

        [XmlElement("className")]
        public string ClassName { get; set; }

        [XmlElement("classNotes")]
        public string ClassNotes { get; set; }

        [XmlElement("definitionMandatoryClass")]
        public string DefinitionMandatoryClass { get; set; }

        [XmlElement("pronunciationMandatoryClass")]
        public string PronunciationMandatoryClass { get; set; }

        [XmlElement("classPattern")]
        public string ClassPattern { get; set; }

        [XmlElement("classGloss")]
        public string ClassGloss { get; set; }
    }
}