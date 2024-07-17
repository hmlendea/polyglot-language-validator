using NuciDAL.DataObjects;

namespace PolyglotLanguageValidator.Models
{
    public sealed class Sentence : EntityBase
    {
        public string Words { get; set; }

        public string ConstructedLanguageSentence { get; set; }

        public string LocalLanguageSentence { get; set; }
    }
}