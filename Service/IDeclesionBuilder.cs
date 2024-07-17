using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public interface IDeclesionBuilder
    {
        public string GetWord(Word word);

        public string GetSentence(string sentence);

        public string GetSentence(params string[] words);
    }
}