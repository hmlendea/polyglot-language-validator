using NUnit.Framework;
using PolyglotLanguageValidator.Configuration;

namespace PolyglotLanguageValidator.UnitTests.Configuration
{
    [TestFixture]
    public sealed class InputSettingsTests
    {
        [TestCase("--dictionary", "dictionary.xml", "--words", "words.json", "--sentences", "sentences.json")]
        [TestCase("-d", "dictionary.xml", "-w", "words.json", "-s", "sentences.json")]
        [TestCase("--dictionary", "dict.xml", "--words", "word.json", "--sentences", "sentence.json")]
        public void GivenLongOrShortArguments_WhenConstructingSettings_ThenAllPathsAreParsed(
            string dictionaryArgument,
            string dictionaryPath,
            string wordsArgument,
            string wordsPath,
            string sentencesArgument,
            string sentencesPath)
        {
            InputSettings settings = new([
                dictionaryArgument, dictionaryPath,
                wordsArgument, wordsPath,
                sentencesArgument, sentencesPath
            ]);

            Assert.That(settings.PolyglotDictionaryFilePath, Is.EqualTo(dictionaryPath));
            Assert.That(settings.WordsFilePath, Is.EqualTo(wordsPath));
            Assert.That(settings.SentencesFilePath, Is.EqualTo(sentencesPath));
        }

        [TestCase("--dictionary", "dictionary.xml", "--words", "words.json")]
        [TestCase("-d", "dict.xml", "-w", "word.json")]
        public void GivenNoSentencesArgument_WhenConstructingSettings_ThenSentencePathIsNull(
            string dictionaryArgument,
            string dictionaryPath,
            string wordsArgument,
            string wordsPath)
        {
            InputSettings settings = new([
                dictionaryArgument, dictionaryPath,
                wordsArgument, wordsPath
            ]);

            Assert.That(settings.SentencesFilePath, Is.Null);
        }
    }
}
