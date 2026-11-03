using System;
using Moq;
using NUnit.Framework;
using NuciDAL.Repositories;
using PolyglotLanguageValidator.Models;
using PolyglotLanguageValidator.Service;

namespace PolyglotLanguageValidator.UnitTests.Service
{
    [TestFixture]
    public sealed class LanguageValidatorTests
    {
        [Test]
        public void GivenNoSentences_WhenTesting_ThenValidationSucceeds()
        {
            Mock<IDeclesionBuilder> declesionBuilderMock = new();
            Mock<IRepository<Sentence>> sentenceRepositoryMock = new();
            sentenceRepositoryMock.Setup(repository => repository.GetAll()).Returns([]);
            LanguageValidator validator = new(declesionBuilderMock.Object, sentenceRepositoryMock.Object);

            Assert.That(() => validator.Test(), Throws.Nothing);
        }

        [TestCase("fel", "fel")]
        [TestCase("feli", "feli")]
        [TestCase("fel dom", "fel dom")]
        public void GivenMatchingSentences_WhenTesting_ThenValidationSucceeds(string generatedSentence, string expectedSentence)
        {
            Sentence sentence = BuildSentence("w1", expectedSentence);
            Mock<IDeclesionBuilder> declesionBuilderMock = new();
            Mock<IRepository<Sentence>> sentenceRepositoryMock = new();
            sentenceRepositoryMock.Setup(repository => repository.GetAll()).Returns([sentence]);
            declesionBuilderMock.Setup(builder => builder.GetSentence(sentence.Words)).Returns(generatedSentence);
            LanguageValidator validator = new(declesionBuilderMock.Object, sentenceRepositoryMock.Object);

            Assert.That(() => validator.Test(), Throws.Nothing);
        }

        [TestCase("feli", "fel")]
        [TestCase("fel dom", "dom fel")]
        [TestCase("", "fel")]
        public void GivenAMismatchingSentence_WhenTesting_ThenAnAggregateExceptionIsThrown(string generatedSentence, string expectedSentence)
        {
            Sentence sentence = BuildSentence("w1", expectedSentence);
            Mock<IDeclesionBuilder> declesionBuilderMock = new();
            Mock<IRepository<Sentence>> sentenceRepositoryMock = new();
            sentenceRepositoryMock.Setup(repository => repository.GetAll()).Returns([sentence]);
            declesionBuilderMock.Setup(builder => builder.GetSentence(sentence.Words)).Returns(generatedSentence);
            LanguageValidator validator = new(declesionBuilderMock.Object, sentenceRepositoryMock.Object);

            AggregateException exception = Assert.Throws<AggregateException>(() => validator.Test());

            Assert.That(exception.InnerExceptions, Has.Count.EqualTo(1));
            Assert.That(exception.InnerExceptions[0].Message, Does.Contain(generatedSentence));
            Assert.That(exception.InnerExceptions[0].Message, Does.Contain(expectedSentence));
        }

        [TestCase("w1", "fel", "fel", false)]
        [TestCase("w2", "feli", "fel", true)]
        [TestCase("w3", "dom", "dom", false)]
        public void GivenMultipleSentences_WhenTesting_ThenEverySentenceIsBuiltAndCompared(
            string firstWordIdentifier,
            string firstGeneratedSentence,
            string secondExpectedSentence,
            bool expectedMismatch)
        {
            Sentence firstSentence = BuildSentence(firstWordIdentifier, firstGeneratedSentence);
            Sentence secondSentence = BuildSentence("w2", secondExpectedSentence);
            Mock<IDeclesionBuilder> declesionBuilderMock = new();
            Mock<IRepository<Sentence>> sentenceRepositoryMock = new();
            sentenceRepositoryMock.Setup(repository => repository.GetAll()).Returns([firstSentence, secondSentence]);
            declesionBuilderMock.Setup(builder => builder.GetSentence(firstSentence.Words)).Returns(firstGeneratedSentence);
            declesionBuilderMock.Setup(builder => builder.GetSentence(secondSentence.Words)).Returns(firstGeneratedSentence);
            LanguageValidator validator = new(declesionBuilderMock.Object, sentenceRepositoryMock.Object);

            if (expectedMismatch)
            {
                AggregateException exception = Assert.Throws<AggregateException>(() => validator.Test());
                Assert.That(exception.InnerExceptions, Has.Count.EqualTo(1));
            }
            else
            {
                Assert.That(() => validator.Test(), Throws.Nothing);
            }

            Times expectedFirstSentenceCalls = firstSentence.Words.Equals(secondSentence.Words)
                ? Times.Exactly(2)
                : Times.Once();
            declesionBuilderMock.Verify(builder => builder.GetSentence(firstSentence.Words), expectedFirstSentenceCalls);
            if (!firstSentence.Words.Equals(secondSentence.Words))
            {
                declesionBuilderMock.Verify(builder => builder.GetSentence(secondSentence.Words), Times.Once());
            }
        }

        private static Sentence BuildSentence(string words, string constructedLanguageSentence)
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                Words = words,
                ConstructedLanguageSentence = constructedLanguageSentence
            };
    }
}
