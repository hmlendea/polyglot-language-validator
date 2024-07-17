using System.Collections.Generic;
using PolyglotLanguageValidator.Models.Polyglot;

namespace PolyglotLanguageValidator.Service
{
    public interface ILanguageParser
    {
        public string GetWord(string localWord, int partOfSpeechId);

        public PolyglotClass GetPartOfSpeech(int id);

        public PolyglotClass GetPartOfSpeech(string name);

        public IEnumerable<PolyglotDeclesionNode> GetDeclesionNodes(int partOfSpeechId);

        public PolyglotDeclesionNode GetDeclesionNode(string name, int partOfSpeechId);

        public PolyglotDimensionNode GetDimensionNode(string declesionNodeName, int partOfSpeechId, string dimensionValue);

        public IEnumerable<PolyglotDeclesionGenerationRule> GetDeclesionGenerationRules(string ruleCombination, int partOfSpeechId);
    }
}