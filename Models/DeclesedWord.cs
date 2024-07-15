using System.Collections.Generic;

namespace PolyglotTester.Models
{
    public sealed class DeclesedWord
    {
        public string LocalWord { get; set; }

        public string PartOfSpeechName { get; set; }

        public Dictionary<string, string> Dimensions { get; set; }
    }
}