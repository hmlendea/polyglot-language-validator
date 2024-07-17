using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NuciDAL.Repositories;
using PolyglotTester.Models;
using PolyglotTester.Models.Polyglot;

namespace PolyglotTester.Service
{
    public sealed class DeclesionBuilder(
        ILanguageParser languageService,
        IRepository<Word> declesedWordsRepository) : IDeclesionBuilder
    {
        public string GetWord(Word word)
        {
            PolyglotClass partOfSpeech = languageService.GetPartOfSpeech(word.PartOfSpeechName);
            string declesedWord = languageService.GetWord(word.LocalWord, partOfSpeech.Id);

            string ruleCombination = ",";
            foreach (var node in languageService.GetDeclesionNodes(partOfSpeech.Id))
            {
                PolyglotDimensionNode dimension = languageService.GetDimensionNode(node.Name, partOfSpeech.Id, word.Dimensions[node.Name]);
                ruleCombination += $"{dimension.Id},";
            }

            foreach (PolyglotDeclesionGenerationRule genRule in languageService.GetDeclesionGenerationRules(ruleCombination, partOfSpeech.Id))
            {
                if (!Regex.Matches(declesedWord, genRule.RegularExpression).Any())
                {
                    continue;
                }

                foreach (PolyglotDeclesionGenerationRuleTransformation transformation in genRule.Transformations)
                {
                    declesedWord = Regex.Replace(declesedWord, transformation.Pattern, transformation.Replacement);
                }
            }

            return declesedWord;
        }

        public string GetSentence(IEnumerable<string> wordIds)
        {
            IList<string> sentenceWords = [];

            foreach (var declesedWordId in wordIds)
            {
                Word declesedWord = declesedWordsRepository.Get(declesedWordId);
                sentenceWords.Add(GetWord(declesedWord));
            }

            return string.Join(' ', sentenceWords);
        }
    }
}