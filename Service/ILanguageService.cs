using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public interface ILanguageService
    {
        public void Load(string path);

        public string GetWord(string localWord, string partOfSpeech);

        public PolyglotClass GetPartOfSpeech(int id);
    }
}