using System;
using Microsoft.Extensions.DependencyInjection;
using PolyglotTester.Service;

namespace PolyglotTester
{
    public class Program
    {
        static IServiceProvider serviceProvider;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            BuildServiceProvider();

            ILanguageService language = serviceProvider.GetService<ILanguageService>();
            language.Load("/home/horatiu/PolyGlot/nucian-language/PGDictionary.xml");

            Console.WriteLine(language.GetWord("knee", "noun"));
            Console.WriteLine(language.GetWord("run", "verb"));

            language.Test();

            BuildServiceProvider();
        }

        static void BuildServiceProvider()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<ILanguageService, LanguageService>()
                .BuildServiceProvider();
        }
    }
}
