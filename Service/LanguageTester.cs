using System;
using NuciDAL.Repositories;
using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public sealed class LanguageTester(
        IDeclesionBuilder declesionService,
        IRepository<Sentence> sentenceRepository) : ILanguageTester
    {
        public void Test()
        {
            foreach (Sentence sentence in sentenceRepository.GetAll())
            {
                string declesedSentence = declesionService.GetSentence(sentence.Words);
                Console.WriteLine(declesedSentence);
            }
        }
    }
}