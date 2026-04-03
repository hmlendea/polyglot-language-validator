using System.Collections.Generic;
using NuciCLI.Arguments;

namespace PolyglotLanguageValidator.Configuration
{
    public sealed class InputSettings
    {
        const string PolyglotDictionaryArgumentName = "dictionary";

        const string WordsArgumentName = "words";

        const string SentencesArgumentName = "sentences";

        static readonly IReadOnlyDictionary<string, string> ArgumentAliases = new Dictionary<string, string>
        {
            ["-d"] = $"--{PolyglotDictionaryArgumentName}",
            ["-w"] = $"--{WordsArgumentName}",
            ["-s"] = $"--{SentencesArgumentName}"
        };

        public string PolyglotDictionaryFilePath { get; set; }

        public string WordsFilePath { get; set; }

        public string SentencesFilePath { get; set; }

        public InputSettings(string[] args)
        {
            ArgumentParser parser = new();
            parser.AddArgument(PolyglotDictionaryArgumentName, required: true);
            parser.AddArgument(WordsArgumentName, required: true);
            parser.AddArgument(SentencesArgumentName, defaultValue: null);

            ArgumentsCollection parsedArguments = parser.ParseArgs(NormalizeArguments(args));

            PolyglotDictionaryFilePath = parsedArguments.Get<string>(PolyglotDictionaryArgumentName);
            WordsFilePath = parsedArguments.Get<string>(WordsArgumentName);
            SentencesFilePath = parsedArguments.Has(SentencesArgumentName)
                ? parsedArguments.Get<string>(SentencesArgumentName)
                : null;
        }

        static string[] NormalizeArguments(string[] args)
        {
            List<string> normalizedArguments = [];

            foreach (string arg in args)
            {
                normalizedArguments.Add(ArgumentAliases.TryGetValue(arg, out string normalizedArgument)
                    ? normalizedArgument
                    : arg);
            }

            return [.. normalizedArguments];
        }
    }
}