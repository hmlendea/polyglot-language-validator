using System;
using Microsoft.Extensions.DependencyInjection;
using NuciDAL.Repositories;
using PolyglotLanguageValidator.Configuration;
using PolyglotLanguageValidator.Models;
using PolyglotLanguageValidator.Service;

namespace PolyglotLanguageValidator
{
    public class Program
    {
        static IServiceProvider serviceProvider;
        static InputSettings inputSettings;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            inputSettings = new InputSettings(args);

            BuildServiceProvider();

            ILanguageValidator validator = serviceProvider.GetService<ILanguageValidator>();

            try
            {
                validator.Test();
            }
            catch (AggregateException exception)
            {
                foreach (Exception innerException in exception.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }

                Environment.Exit(1);
            }
        }

        static void BuildServiceProvider()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton(inputSettings)
                .AddSingleton<IDeclesionBuilder, DeclesionBuilder>()
                .AddSingleton<ILanguageParser, LanguageParser>()
                .AddSingleton<ILanguageValidator, LanguageValidator>()
                .AddSingleton<IRepository<Word>>(x => new JsonRepository<Word>(inputSettings.WordsFilePath))
                .AddSingleton<IRepository<Sentence>>(x => new JsonRepository<Sentence>(inputSettings.SentencesFilePath))
                .BuildServiceProvider();
        }
    }
}
