# Flows — Startup and validation

## Application startup flow

```
Program.Main(args)
    │
    ▼
InputSettings(args)
    │
    ├─► NormaliseArguments(args)  // -d → --dictionary, etc.
    │
    ├─► ArgumentParser.ParseArgs()
    │       ├─► --dictionary (required)
    │       ├─► --words (required)
    │       └─► --sentences (optional, default null)
    │
    ▼
BuildServiceProvider()
    │
    ├─► ServiceCollection
    │       ├─► AddSingleton(inputSettings)
    │       ├─► AddSingleton<IDeclesionBuilder, DeclesionBuilder>()
    │       ├─► AddSingleton<ILanguageParser, LanguageParser>()
    │       ├─► AddSingleton<ILanguageValidator, LanguageValidator>()
    │       ├─► AddSingleton<IFileRepository<Word>>(JsonRepository<Word>)
    │       └─► AddSingleton<IFileRepository<Sentence>>(JsonRepository<Sentence>)
    │
    ▼
serviceProvider.GetService<ILanguageValidator>()
    │
    ▼
validator.Test()
    │
    ▼
LanguageValidator.Test()
    │
    ├─► sentenceRepository.GetAll()  // Load all sentences
    │
    ├─► For each sentence:
    │       │
    │       ▼
    │       declesionService.GetSentence(sentence.Words)
    │       │
    │       ▼
    │       DeclesionBuilder.GetSentence(words)
    │       │
    │       ├─► Split words by space
    │       │
    │       ├─► For each wordId:
    │       │       │
    │       │       ▼
    │       │       wordRepository.Get(wordId)  // Load Word
    │       │       │
    │       │       ▼
    │       │       DeclesionBuilder.GetWord(Word)
    │       │       │
    │       │       ├─► languageService.GetPartOfSpeech(word.PartOfSpeechName)
    │       │       │
    │       │       ├─► languageService.GetWord(word.LocalWord, posId)
    │       │       │
    │       │       ├─► languageService.GetDeclesionNodes(posId)
    │       │       │
    │       │       ├─► For each node:
    │       │       │       │
    │       │       │       ▼
    │       │       │       languageService.GetDimensionNode(node.Name, posId, word.Dimensions[node.Name])
    │       │       │       │
    │       │       │       ▼
    │       │       │       Build ruleCombination (",dimId1,dimId2,...")
    │       │       │
    │       │       ├─► languageService.GetDeclesionGenerationRules(ruleCombination, posId)
    │       │       │
    │       │       ├─► For each matching rule:
    │       │       │       │
    │       │       │       ▼
    │       │       │       Regex.Matches(word, rule.RegularExpression)
    │       │       │       │
    │       │       │       ▼ (if match)
    │       │       │       For each transformation:
    │       │       │               Regex.Replace(word, transformation.Pattern, transformation.Replacement)
    │       │       │
    │       │       ▼
    │       │       Return constructed word
    │       │
    │       ▼
    │       Join words with space
    │
    ▼
Compare generated vs sentence.ConstructedLanguageSentence
    │
    ├─► Match → continue
    └─► Mismatch → Add MismatchingValueException to list
    │
    ▼
After all sentences:
    │
    ├─► Any mismatches?
    │       ├─► Yes → throw AggregateException("Sentence validation failed.", exceptions)
    │       └─► No → return (success)
    │
    ▼
Program.Main catches AggregateException
    │
    ├─► Print each inner exception message
    │
    ▼
Environment.Exit(1)
```

## Sentence validation flow (detail)

```
LanguageValidator.Test()
    │
    ▼
sentenceRepository.GetAll()
    │
    ▼
[Sentence{Id="s1", Words="w1 w2", ConstructedLanguageSentence="fel feli", ...}, ...]
    │
    ▼
foreach sentence:
    │
    ▼
declesionBuilder.GetSentence("w1 w2")
    │
    ▼
DeclesionBuilder.GetSentence("w1 w2")
    │
    ├─► Split: ["w1", "w2"]
    │
    ├─► wordRepository.Get("w1")
    │       │
    │       ▼
    │       Word{Id="w1", LocalWord="cat", PartOfSpeechName="noun", Dimensions={"number":"singular"}}
    │
    ├─► GetWord(Word)
    │       │
    │       ▼
    │       languageParser.GetPartOfSpeech("noun")
    │       │       │
    │       │       ▼
    │       │       PolyglotClass{Id=1, Name="noun"}
    │       │
    │       ▼
    │       languageParser.GetWord("cat", 1)
    │       │       │
    │       │       ▼
    │       │       PolyglotWord{LocalWord="cat, cats", ConstructedWord="fel", WordTypeId=1}
    │       │       │
    │       │       ▼
    │       │       Returns "fel"
    │       │
    │       ▼
    │       languageParser.GetDeclesionNodes(1)
    │       │       │
    │       │       ▼
    │       │       [PolyglotDeclesionNode{Id=1, Name="number", PartOfSpeechId=1, Dimensions=[{Id=1,Name="singular"},{Id=2,Name="plural"}]}]
    │       │
    │       ▼
    │       For node "number":
    │               languageParser.GetDimensionNode("number", 1, "singular")
    │               │
    │               ▼
    │               PolyglotDimensionNode{Id=1, Name="singular"}
    │               │
    │               ▼
    │               ruleCombination = ",1,"
    │       │
    │       ▼
    │       languageParser.GetDeclesionGenerationRules(",1,", 1)
    │       │       │
    │       │       ▼
    │       │       [PolyglotDeclesionGenerationRule{RuleCombination=",1,", RegularExpression="$", Transformations=[{Pattern="$", Replacement=""}]}]
    │       │
    │       ▼
    │       For each rule:
    │               Regex.Matches("fel", "$") → match
    │               │
    │               ▼
    │               Regex.Replace("fel", "$", "") → "fel" (no change for singular)
    │       │
    │       ▼
    │       Returns "fel"
    │
    ├─► wordRepository.Get("w2")
    │       │
    │       ▼
    │       Word{Id="w2", LocalWord="cats", PartOfSpeechName="noun", Dimensions={"number":"plural"}}
    │
    ├─► GetWord(Word) → similar flow, ruleCombination=",2,"
    │       │
    │       ▼
    │       Regex.Replace("fel", "$", "i") → "feli"
    │       │
    │       ▼
    │       Returns "feli"
    │
    ▼
Join: "fel feli"
    │
    ▼
Compare with sentence.ConstructedLanguageSentence ("fel feli")
    │
    ├─► Equal → no exception
    └─► Not equal → MismatchingValueException("fel feli", "expected")
```

## Dictionary loading flow

```
LanguageParser constructor(InputSettings)
    │
    ▼
LoadLanguage(inputSettings.PolyglotDictionaryFilePath)
    │
    ├─► File.ReadAllText(path)
    │
    ├─► XmlSerializer(typeof(PolyglotDictionary))
    │
    ├─► StringReader(xml)
    │
    ▼
Deserialize → PolyglotDictionary
    │
    ├─► PartsOfSpeech.Classes → List<PolyglotClass>
    ├─► Lexicon.Words → List<PolyglotWord>
    └─► DeclesionCollection
            ├─► Nodes → List<PolyglotDeclesionNode>
            └─► DeclesionGenerationRules → List<PolyglotDeclesionGenerationRule>
    │
    ▼
Store in readonly field 'language'
```

## Word lookup flow

```
languageParser.GetWord("cat", 1)
    │
    ▼
language.Lexicon.Words.First(x =>
    x.WordTypeId == 1 &&
    x.LocalWord.Split(',').Select(x => x.Trim()).Contains("cat"))
    │
    ▼
PolyglotWord{WordId=1, LocalWord="cat, cats", ConstructedWord="fel", WordTypeId=1}
    │
    ▼
Return "fel"
```

## Rule matching flow

```
languageParser.GetDeclesionGenerationRules(",1,", 1)
    │
    ▼
language.DeclesionCollection.DeclesionGenerationRules.Where(x =>
    x.RuleCombination == ",1," &&
    x.PartOfSpeechId == 1)
    │
    ▼
Returns matching rules (ordered by XML)
    │
    ▼
DeclesionBuilder applies each rule:
    │
    ├─► Regex.Matches(currentWord, rule.RegularExpression)
    │
    ├─► If match:
    │       For each transformation in rule.Transformations:
    │               currentWord = Regex.Replace(currentWord, transformation.Pattern, transformation.Replacement)
    │
    ▼
Return transformed word
```