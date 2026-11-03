using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NuciDAL.Repositories;
using PolyglotLanguageValidator.Models;
using PolyglotLanguageValidator.Models.Polyglot;
using PolyglotLanguageValidator.Service;

namespace PolyglotLanguageValidator.UnitTests.Service
{
    [TestFixture]
    public sealed class DeclesionBuilderTests
    {
        private Mock<ILanguageParser> languageParserMock;
        private Mock<IRepository<Word>> wordRepositoryMock;
        private DeclesionBuilder declesionBuilder;

        [SetUp]
        public void SetUp()
        {
            languageParserMock = new Mock<ILanguageParser>();
            wordRepositoryMock = new Mock<IRepository<Word>>();
            declesionBuilder = new DeclesionBuilder(languageParserMock.Object, wordRepositoryMock.Object);
        }

        [TestCase("cat", "fel", "noun", "singular", "fel")]
        [TestCase("cat", "fel", "noun", "plural", "feli")]
        [TestCase("dog", "can", "noun", "singular", "can")]
        [TestCase("dog", "can", "noun", "plural", "cani")]
        public void GivenAWord_WhenGettingTheWord_ThenTheExpectedConstructedWordIsReturned(
            string localWord,
            string constructedWord,
            string partOfSpeechName,
            string dimensionValue,
            string expectedWord)
        {
            PolyglotClass partOfSpeech = BuildPartOfSpeech();
            PolyglotDeclesionNode declesionNode = BuildDeclesionNode();
            PolyglotDimensionNode dimension = new() { Id = dimensionValue.Equals("plural") ? 2 : 1, Name = dimensionValue };
            PolyglotDeclesionGenerationRule generationRule = BuildGenerationRule(dimension.Id, dimensionValue);
            Word word = BuildWord(localWord, partOfSpeechName, dimensionValue);

            languageParserMock.Setup(parser => parser.GetPartOfSpeech(partOfSpeechName)).Returns(partOfSpeech);
            languageParserMock.Setup(parser => parser.GetWord(localWord, partOfSpeech.Id)).Returns(constructedWord);
            languageParserMock.Setup(parser => parser.GetDeclesionNodes(partOfSpeech.Id)).Returns([declesionNode]);
            languageParserMock.Setup(parser => parser.GetDimensionNode(declesionNode.Name, partOfSpeech.Id, dimensionValue)).Returns(dimension);
            languageParserMock.Setup(parser => parser.GetDeclesionGenerationRules($",{dimension.Id},", partOfSpeech.Id)).Returns([generationRule]);

            string actualWord = declesionBuilder.GetWord(word);

            Assert.That(actualWord, Is.EqualTo(expectedWord));
        }

        [TestCase("cat", "fel", "noun", "singular")]
        [TestCase("dog", "can", "noun", "plural")]
        public void GivenAWord_WhenGettingTheWord_ThenTheParserReceivesTheWordDefinition(
            string localWord,
            string constructedWord,
            string partOfSpeechName,
            string dimensionValue)
        {
            PolyglotClass partOfSpeech = BuildPartOfSpeech();
            PolyglotDeclesionNode declesionNode = BuildDeclesionNode();
            PolyglotDimensionNode dimension = new() { Id = 1, Name = dimensionValue };
            Word word = BuildWord(localWord, partOfSpeechName, dimensionValue);

            languageParserMock.Setup(parser => parser.GetPartOfSpeech(partOfSpeechName)).Returns(partOfSpeech);
            languageParserMock.Setup(parser => parser.GetWord(localWord, partOfSpeech.Id)).Returns(constructedWord);
            languageParserMock.Setup(parser => parser.GetDeclesionNodes(partOfSpeech.Id)).Returns([declesionNode]);
            languageParserMock.Setup(parser => parser.GetDimensionNode(declesionNode.Name, partOfSpeech.Id, dimensionValue)).Returns(dimension);
            languageParserMock.Setup(parser => parser.GetDeclesionGenerationRules(",1,", partOfSpeech.Id)).Returns([]);

            declesionBuilder.GetWord(word);

            languageParserMock.Verify(parser => parser.GetPartOfSpeech(partOfSpeechName), Times.Once);
            languageParserMock.Verify(parser => parser.GetWord(localWord, partOfSpeech.Id), Times.Once);
            languageParserMock.Verify(parser => parser.GetDimensionNode(declesionNode.Name, partOfSpeech.Id, dimensionValue), Times.Once);
        }

        [TestCase("w1", "fel")]
        [TestCase("w2", "cani")]
        [TestCase("w3", "dom")]
        public void GivenAWordIdentifier_WhenGettingASentence_ThenTheWordIsLoadedAndBuilt(string wordIdentifier, string expectedWord)
        {
            Word word = BuildWord("cat", "noun", "singular");
            wordRepositoryMock.Setup(repository => repository.Get(wordIdentifier)).Returns(word);
            ConfigureWord(word, expectedWord);

            string actualSentence = declesionBuilder.GetSentence(wordIdentifier);

            Assert.That(actualSentence, Is.EqualTo(expectedWord));
            wordRepositoryMock.Verify(repository => repository.Get(wordIdentifier), Times.Once);
        }

        [TestCase("w1 w2", "fel fel")]
        [TestCase("w1 w2 w3", "fel fel fel")]
        [TestCase("w1", "fel")]
        public void GivenASentence_WhenGettingTheSentence_ThenWordsAreJoinedInInputOrder(string sentence, string expectedSentence)
        {
            string[] wordIdentifiers = sentence.Split(' ');
            foreach (string wordIdentifier in wordIdentifiers)
            {
                wordRepositoryMock.Setup(repository => repository.Get(wordIdentifier)).Returns(BuildWord("cat", "noun", "singular"));
            }

            ConfigureWord(BuildWord("cat", "noun", "singular"), "fel");

            string actualSentence = declesionBuilder.GetSentence(sentence);

            Assert.That(actualSentence, Is.EqualTo(expectedSentence));
        }

        [TestCase("w1", "fel")]
        [TestCase("w1", "fel")]
        public void GivenWordArguments_WhenGettingTheSentence_ThenTheWordsAreJoined(string firstWordIdentifier, string expectedSentence)
        {
            wordRepositoryMock.Setup(repository => repository.Get(firstWordIdentifier)).Returns(BuildWord("cat", "noun", "singular"));
            ConfigureWord(BuildWord("cat", "noun", "singular"), expectedSentence);

            string actualSentence = declesionBuilder.GetSentence(firstWordIdentifier);

            Assert.That(actualSentence, Is.EqualTo(expectedSentence));
        }

        private void ConfigureWord(Word word, string constructedWord)
        {
            PolyglotClass partOfSpeech = BuildPartOfSpeech();
            PolyglotDeclesionNode declesionNode = BuildDeclesionNode();
            PolyglotDimensionNode dimension = new() { Id = 1, Name = "singular" };

            languageParserMock.Setup(parser => parser.GetPartOfSpeech(word.PartOfSpeechName)).Returns(partOfSpeech);
            languageParserMock.Setup(parser => parser.GetWord(word.LocalWord, partOfSpeech.Id)).Returns(constructedWord);
            languageParserMock.Setup(parser => parser.GetDeclesionNodes(partOfSpeech.Id)).Returns([declesionNode]);
            languageParserMock.Setup(parser => parser.GetDimensionNode(declesionNode.Name, partOfSpeech.Id, "singular")).Returns(dimension);
            languageParserMock.Setup(parser => parser.GetDeclesionGenerationRules(",1,", partOfSpeech.Id)).Returns([]);
        }

        private static PolyglotClass BuildPartOfSpeech()
            => new() { Id = 1, Name = "noun" };

        private static PolyglotDeclesionNode BuildDeclesionNode()
            => new() { Id = 1, Name = "number", PartOfSpeechId = 1 };

        private static PolyglotDeclesionGenerationRule BuildGenerationRule(int dimensionId, string dimensionValue)
            => new()
            {
                RuleCombination = $",{dimensionId},",
                RegularExpression = dimensionValue.Equals("plural") ? "$" : "^$",
                PartOfSpeechId = 1,
                Transformations = [new() { Pattern = "$", Replacement = "i" }]
            };

        private static Word BuildWord(string localWord, string partOfSpeechName, string dimensionValue)
            => new()
            {
                Id = "word-id",
                LocalWord = localWord,
                PartOfSpeechName = partOfSpeechName,
                Dimensions = new Dictionary<string, string> { ["number"] = dimensionValue }
            };
    }
}
