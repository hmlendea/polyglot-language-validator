using System;
using System.Collections.Generic;

namespace PolyglotTester.Service
{
    public sealed class LanguageTester(IDeclesionBuilder declesionService) : ILanguageTester
    {
        public void Test()
        {
            foreach (string sentenceToDeclese in new List<string>()
                {
                    "eggs with sour_cream",
                    "horace has apples_accusative",
                    "the_church is here",
                    "the_duck_male dies of hunger",
                    "the_fishermen are_fishing in the_lakes",
                    "the_villagers plant_verb carrots in the_garden",
                    "the_worker builds houses_accusative",
                    "we_buy muffins or watermelons"
                })
            {
                string declesedSentence = declesionService.GetSentence(sentenceToDeclese.Split(' '));
                Console.WriteLine(declesedSentence);
            }
        }
    }
}