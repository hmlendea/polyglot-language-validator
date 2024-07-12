using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public sealed class LanguageService : ILanguageService
    {
        PolyglotDictionary language;

        public void Load(string path)
        {
            XmlSerializer serializer = new(typeof(PolyglotDictionary));
            string xml = File.ReadAllText(path);

            using StringReader reader = new(xml);
            language = (PolyglotDictionary)serializer.Deserialize(reader);
        }

        public string GetWord(string localWord, string partOfSpeech)
        {
            PolyglotClass partOfSpeechClass = language.PartsOfSpeech.Classes.First(x => x.Name.Equals(partOfSpeech));
            PolyglotWord word = language.Lexicon.Words.First(x =>
                x.WordTypeId.Equals(partOfSpeechClass.Id) &&
                x.LocalWord.Split(',').Select(x => x.Trim()).Contains(localWord));

            return word.ConstructedWord;
        }

        public PolyglotClass GetPartOfSpeech(int id)
            => language.PartsOfSpeech.Classes.First(x => x.Id.Equals(id));
    }
}