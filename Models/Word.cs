using System.Collections.Generic;
using NuciDAL.DataObjects;

namespace PolyglotLanguageValidator.Models
{
    public sealed class Word : EntityBase
    {
        public string LocalWord { get; set; }

        public string PartOfSpeechName { get; set; }

        public Dictionary<string, string> Dimensions { get; set; }
    }
}