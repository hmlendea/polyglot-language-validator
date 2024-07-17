using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using PolyglotTester.Models.Polyglot;

namespace PolyglotTester.Service
{
    public sealed class LanguageParser : ILanguageParser
    {
        readonly PolyglotDictionary language;

        public LanguageParser()
        {
            language = LoadLanguage("/home/horatiu/PolyGlot/nucian-language/PGDictionary.xml");
        }

        public string GetWord(string localWord, int partOfSpeechId)
        {
            PolyglotClass partOfSpeechClass = GetPartOfSpeech(partOfSpeechId);
            PolyglotWord word = language.Lexicon.Words.First(x =>
                x.WordTypeId.Equals(partOfSpeechClass.Id) &&
                x.LocalWord.Split(',').Select(x => x.Trim()).Contains(localWord));

            return word.ConstructedWord;
        }

        public PolyglotClass GetPartOfSpeech(int id)
            => language.PartsOfSpeech.Classes.First(x => x.Id.Equals(id));

        public PolyglotClass GetPartOfSpeech(string name)
            => language.PartsOfSpeech.Classes.First(x => x.Name.Equals(name));

        public IEnumerable<PolyglotDeclesionNode> GetDeclesionNodes(int partOfSpeechId)
            => language.DeclesionCollection.Nodes.Where(x => x.PartOfSpeechId.Equals(partOfSpeechId));

        public PolyglotDeclesionNode GetDeclesionNode(string name, int partOfSpeechId)
            => GetDeclesionNodes(partOfSpeechId).First(x => x.Name.Equals(name));

        public PolyglotDimensionNode GetDimensionNode(string declesionNodeName, int partOfSpeechId, string dimensionValue)
        {
            PolyglotDeclesionNode declesionNode = GetDeclesionNode(declesionNodeName, partOfSpeechId);
            PolyglotDimensionNode dimensionNode = declesionNode?.Dimensions.FirstOrDefault(x => x.Name.Equals(dimensionValue));

            return dimensionNode;
        }

        public IEnumerable<PolyglotDeclesionGenerationRule> GetDeclesionGenerationRules(string ruleCombination, int partOfSpeechId)
            => language.DeclesionCollection.DeclesionGenerationRules.Where(x =>
                x.RuleCombination.Equals(ruleCombination) &&
                x.PartOfSpeechId.Equals(partOfSpeechId));

        private PolyglotDictionary LoadLanguage(string path)
        {
            XmlSerializer serializer = new(typeof(PolyglotDictionary));
            string xml = File.ReadAllText(path);

            using StringReader reader = new(xml);
            return (PolyglotDictionary)serializer.Deserialize(reader);
        }
    }
}