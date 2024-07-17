using System;
using System.Collections.Generic;
using System.Linq;
using NuciDAL.Repositories;
using PolyglotTester.Exceptions;
using PolyglotTester.Models;

namespace PolyglotTester.Service
{
    public sealed class LanguageTester(
        IDeclesionBuilder declesionService,
        IRepository<Sentence> sentenceRepository) : ILanguageTester
    {
        public void Test()
        {
            IList<MismatchingValueException> exceptions = [];

            foreach (Sentence sentence in sentenceRepository.GetAll())
            {
                string declesedSentence = declesionService.GetSentence(sentence.Words);

                if (!sentence.ConstructedLanguageSentence.Equals(declesedSentence))
                {
                    exceptions.Add(new MismatchingValueException(declesedSentence, sentence.ConstructedLanguageSentence));
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException("Sentence validation failed.", exceptions);
            }
        }
    }
}