using NuciCLI;

namespace PolyglotLanguageValidator.Configuration
{
    public sealed class InputSettings
    {
        static readonly string[] PolyglotDictionaryFilePathOptions = ["-d", "--dictionary"];

        static readonly string[] WordsFilePathOptions = ["-w", "--words"];

        static readonly string[] SentencesFilePathOptions = ["-s", "--sentences"];

        public string PolyglotDictionaryFilePath { get; set; }

        public string WordsFilePath { get; set; }

        public string SentencesFilePath { get; set; }

        public InputSettings(string[] args)
        {
            PolyglotDictionaryFilePath = CliArgumentsReader.GetOptionValue(args, PolyglotDictionaryFilePathOptions);
            WordsFilePath = CliArgumentsReader.GetOptionValue(args, WordsFilePathOptions);
            SentencesFilePath = CliArgumentsReader.TryGetOptionValue(args, SentencesFilePathOptions);
        }
    }
}