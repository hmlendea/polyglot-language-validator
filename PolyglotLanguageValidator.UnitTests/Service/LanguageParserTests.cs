using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PolyglotLanguageValidator.Configuration;
using PolyglotLanguageValidator.Models.Polyglot;
using PolyglotLanguageValidator.Service;

namespace PolyglotLanguageValidator.UnitTests.Service
{
    [TestFixture]
    public sealed class LanguageParserTests
    {
        private const string DictionaryFileName = "dictionary.xml";
        private string dictionaryFilePath;

        [SetUp]
        public void SetUp()
        {
            dictionaryFilePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, DictionaryFileName);
            File.WriteAllText(dictionaryFilePath, BuildDictionaryXml());
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(dictionaryFilePath))
            {
                File.Delete(dictionaryFilePath);
            }
        }

        [TestCase(1, "noun")]
        [TestCase(2, "verb")]
        public void GivenAPartOfSpeechId_WhenGettingThePartOfSpeech_ThenTheMatchingClassIsReturned(int identifier, string expectedName)
        {
            LanguageParser parser = BuildParser();

            PolyglotClass partOfSpeech = parser.GetPartOfSpeech(identifier);

            Assert.That(partOfSpeech.Name, Is.EqualTo(expectedName));
        }

        [TestCase("noun", 1)]
        [TestCase("verb", 2)]
        public void GivenAPartOfSpeechName_WhenGettingThePartOfSpeech_ThenTheMatchingClassIsReturned(string name, int expectedIdentifier)
        {
            LanguageParser parser = BuildParser();

            PolyglotClass partOfSpeech = parser.GetPartOfSpeech(name);

            Assert.That(partOfSpeech.Id, Is.EqualTo(expectedIdentifier));
        }

        [TestCase("cat", 1, "fel")]
        [TestCase("cats", 1, "fel")]
        [TestCase("run", 2, "cur")]
        public void GivenALocalWordAndPartOfSpeech_WhenGettingTheWord_ThenTheConstructedWordIsReturned(string localWord, int partOfSpeechIdentifier, string expectedWord)
        {
            LanguageParser parser = BuildParser();

            string constructedWord = parser.GetWord(localWord, partOfSpeechIdentifier);

            Assert.That(constructedWord, Is.EqualTo(expectedWord));
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(1, 1)]
        public void GivenAPartOfSpeech_WhenGettingDeclensionNodes_ThenOnlyRelatedNodesAreReturned(int partOfSpeechIdentifier, int expectedCount)
        {
            LanguageParser parser = BuildParser();

            IEnumerable<PolyglotDeclesionNode> nodes = parser.GetDeclesionNodes(partOfSpeechIdentifier);

            Assert.That(nodes.Count(), Is.EqualTo(expectedCount));
        }

        [TestCase("number", 1, "plural", 2)]
        [TestCase("tense", 2, "present", 3)]
        public void GivenADeclensionNodeAndDimension_WhenGettingTheDimension_ThenTheMatchingDimensionIsReturned(
            string nodeName,
            int partOfSpeechIdentifier,
            string dimensionName,
            int expectedIdentifier)
        {
            LanguageParser parser = BuildParser();

            PolyglotDimensionNode dimension = parser.GetDimensionNode(nodeName, partOfSpeechIdentifier, dimensionName);

            Assert.That(dimension.Id, Is.EqualTo(expectedIdentifier));
        }

        [TestCase(",1,", 1, 1)]
        [TestCase(",2,", 1, 1)]
        [TestCase(",3,", 2, 1)]
        [TestCase(",999,", 1, 0)]
        public void GivenARuleCombination_WhenGettingGenerationRules_ThenMatchingRulesAreReturned(string ruleCombination, int partOfSpeechIdentifier, int expectedCount)
        {
            LanguageParser parser = BuildParser();

            IEnumerable<PolyglotDeclesionGenerationRule> rules = parser.GetDeclesionGenerationRules(ruleCombination, partOfSpeechIdentifier);

            Assert.That(rules.Count(), Is.EqualTo(expectedCount));
        }

        private LanguageParser BuildParser()
            => new(new InputSettings([
                "--dictionary", dictionaryFilePath,
                "--words", "words.json"
            ]));

        private static string BuildDictionaryXml()
            => """
               <dictionary>
                 <partsOfSpeech>
                   <class><classId>1</classId><className>noun</className></class>
                   <class><classId>2</classId><className>verb</className></class>
                 </partsOfSpeech>
                 <lexicon>
                   <word><wordId>1</wordId><localWord>cat, cats</localWord><conWord>fel</conWord><wordTypeId>1</wordTypeId></word>
                   <word><wordId>2</wordId><localWord>cats</localWord><conWord>feli</conWord><wordTypeId>1</wordTypeId></word>
                   <word><wordId>3</wordId><localWord>run</localWord><conWord>cur</conWord><wordTypeId>2</wordTypeId></word>
                 </lexicon>
                 <declensionCollection>
                   <declensionNode><declensionId>1</declensionId><declensionText>number</declensionText><declensionRelatedId>1</declensionRelatedId>
                     <dimensionNode><dimensionId>1</dimensionId><dimensionName>singular</dimensionName></dimensionNode>
                     <dimensionNode><dimensionId>2</dimensionId><dimensionName>plural</dimensionName></dimensionNode>
                   </declensionNode>
                   <declensionNode><declensionId>2</declensionId><declensionText>tense</declensionText><declensionRelatedId>2</declensionRelatedId>
                     <dimensionNode><dimensionId>3</dimensionId><dimensionName>present</dimensionName></dimensionNode>
                   </declensionNode>
                   <decGenRule><decGenRuleComb>,1,</decGenRuleComb><decGenRuleRegex>$</decGenRuleRegex><decGenRuleName>singular</decGenRuleName><decGenRuleTypeId>1</decGenRuleTypeId><decGenRuleIndex>1</decGenRuleIndex></decGenRule>
                   <decGenRule><decGenRuleComb>,2,</decGenRuleComb><decGenRuleRegex>$</decGenRuleRegex><decGenRuleName>plural</decGenRuleName><decGenRuleTypeId>1</decGenRuleTypeId><decGenRuleIndex>1</decGenRuleIndex></decGenRule>
                   <decGenRule><decGenRuleComb>,3,</decGenRuleComb><decGenRuleRegex>$</decGenRuleRegex><decGenRuleName>present</decGenRuleName><decGenRuleTypeId>2</decGenRuleTypeId><decGenRuleIndex>1</decGenRuleIndex></decGenRule>
                 </declensionCollection>
               </dictionary>
               """;
    }
}
