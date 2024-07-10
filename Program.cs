using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using PolyglotTester.Models;

namespace PolyglotTester
{
    public class Program
    {
        public static IServiceProvider ServiceProvider;

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            string xml = File.ReadAllText("/home/horatiu/PolyGlot/nucian-language/PGDictionary.xml");
            XmlSerializer serializer = new(typeof(PolyglotDictionary));

            using (StringReader reader = new(xml))
            {
                PolyglotDictionary dictionary = (PolyglotDictionary)serializer.Deserialize(reader);

                PrintWordFor(dictionary, "noun", "knee");
                PrintWordFor(dictionary, "verb", "run");
            }

            BuildServiceProvider();
        }

        static void BuildServiceProvider()
        {
        }

        static void PrintWordFor(PolyglotDictionary dictionary, string partOfSpeech, string englishWord)
        {
            PolyglotClass partOfSpeechClass = dictionary.PartsOfSpeech.Classes.First(x => x.ClassName.Equals(partOfSpeech));
            PolyglotWord word = dictionary.Lexicon.Words.First(x =>
                x.WordTypeId.Equals(partOfSpeechClass.ClassId) &&
                x.LocalWord.Split(',').Select(x => x.Trim()).Contains(englishWord));

            Console.WriteLine($"{englishWord} ({partOfSpeech}) = {word.ConstructedWord}");
        }
    }
}
