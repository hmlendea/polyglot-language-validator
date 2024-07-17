using System;
using System.Collections.Generic;
using System.Linq;
using NuciDAL.Repositories;
using PolyglotLanguageValidator.Exceptions;
using PolyglotLanguageValidator.Models;

namespace PolyglotLanguageValidator.Service
{
    public sealed class LanguageValidator(
        IDeclesionBuilder declesionService,
        IRepository<Sentence> sentenceRepository) : ILanguageValidator
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