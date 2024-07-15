using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using PolyglotTester.Models;
using PolyglotTester.Models.Polyglot;

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

        public PolyglotClass GetPartOfSpeech(string name)
            => language.PartsOfSpeech.Classes.First(x => x.Name.Equals(name));

        public PolyglotDeclesionNode GetDeclesionNode(string name, int partOfSpeechId)
            => language.DeclesionCollection.Nodes.First(x => x.Name.Equals(name) && x.PartOfSpeechId.Equals(partOfSpeechId));

        public void Test()
        {
            string properNounHorace = GetDeclesedWord(new()
            {
                LocalWord = "Horace",
                PartOfSpeechName = "proper noun"
            });
            string verbHas = GetDeclesedWord(new()
            {
                LocalWord = "have",
                PartOfSpeechName = "verb",
                Dimensions = new()
                {
                    { "Tense", "present" },
                    { "Number", "singular" },
                    { "Persona", "third" }
                }
            });
            string nounApples = GetDeclesedWord(new()
            {
                LocalWord = "apple",
                PartOfSpeechName = "noun",
                Dimensions = new()
                {
                    { "Number", "plural" }
                }
            });

            Console.WriteLine($"{properNounHorace} {verbHas} {nounApples}");

            return;
        }

        public string GetDeclesedWord(DeclesedWord declesedWord)
        {
            PolyglotClass partOfSpeech = language.PartsOfSpeech.Classes.First(x => x.Name.Equals(declesedWord.PartOfSpeechName));
            string word = GetWord(declesedWord.LocalWord, partOfSpeech.Name);

            string ruleCombination = ",";
            foreach (var node in language.DeclesionCollection.Nodes.Where(x => x.PartOfSpeechId.Equals(partOfSpeech.Id)))
            {
                PolyglotDimensionNode dimension = GetDimensionNode(node.Name, partOfSpeech.Id, declesedWord.Dimensions[node.Name]);
                ruleCombination += $"{dimension.Id},";
            }

            foreach (PolyglotDeclesionGenerationRule genRule in language.DeclesionCollection.DeclesionGenerationRules.Where(x => x.RuleCombination.Equals(ruleCombination)))
            {
                if (!Regex.Matches(word, genRule.RegularExpression).Any())
                {
                    continue;
                }

                foreach (PolyglotDeclesionGenerationRuleTransformation transformation in genRule.Transformations)
                {
                    Console.WriteLine(declesedWord.LocalWord + " " + transformation.Pattern + " " + transformation.Replacement);
                    word = Regex.Replace(word, transformation.Pattern, transformation.Replacement);
                }
            }

            return word;
        }

        private PolyglotDimensionNode GetDimensionNode(string declesionNodeName, int partOfSpeechId, string dimensionValue)
        {
            PolyglotDeclesionNode declesionNode = GetDeclesionNode(declesionNodeName, partOfSpeechId);
            PolyglotDimensionNode dimensionNode = declesionNode?.Dimensions.FirstOrDefault(x => x.Name.Equals(dimensionValue));

            return dimensionNode;
        }
    }
}