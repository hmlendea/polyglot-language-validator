using System.Collections.Generic;
using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public interface IDeclesionBuilder
    {
        public string GetWord(Word word);

        public string GetSentence(IEnumerable<string> wordIds);
    }
}