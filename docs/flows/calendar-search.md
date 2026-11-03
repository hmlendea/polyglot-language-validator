# Flows — Dictionary processing and rule application

## Dictionary parsing flow

```
LanguageParser.LoadLanguage(path)
    │
    ▼
XmlSerializer.Deserialize(StringReader(xml))
    │
    ▼
PolyglotDictionary
    │
    ├─► PartsOfSpeech.Classes
    │       [PolyglotClass{Id=1, Name="noun", ...}, PolyglotClass{Id=2, Name="verb", ...}]
    │
    ├─► Lexicon.Words
    │       [PolyglotWord{WordId=1, LocalWord="cat, cats", ConstructedWord="fel", WordTypeId=1}, ...]
    │
    └─► DeclesionCollection
            ├─► Nodes
            │       [PolyglotDeclesionNode{Id=1, Name="number", PartOfSpeechId=1, Dimensions=[...]}, ...]
            │
            └─► DeclesionGenerationRules
                    [PolyglotDeclesionGenerationRule{RuleCombination=",1,", RegularExpression="$", ...}, ...]
```

## Declension node resolution flow

```
languageParser.GetDeclesionNodes(partOfSpeechId)
    │
    ▼
language.DeclesionCollection.Nodes.Where(x => x.PartOfSpeechId == partOfSpeechId)
    │
    ▼
Returns nodes in XML order (critical for ruleCombination order)
    │
    ▼
Example for partOfSpeechId=1 (noun):
    [PolyglotDeclesionNode{Id=1, Name="number", PartOfSpeechId=1, Dimensions=[{Id=1,Name="singular"},{Id=2,Name="plural"}]}]
```

## Dimension node resolution flow

```
languageParser.GetDimensionNode("number", 1, "plural")
    │
    ▼
GetDeclesionNode("number", 1) → PolyglotDeclesionNode{Id=1, Name="number", ...}
    │
    ▼
node.Dimensions.FirstOrDefault(x => x.Name == "plural")
    │
    ▼
PolyglotDimensionNode{Id=2, Name="plural"}
    │
    ▼
Returns dimension with Id=2 (used in ruleCombination)
```

## Rule combination building flow

```
DeclesionBuilder.GetWord(Word{PartOfSpeechName="noun", Dimensions={"number":"plural"}})
    │
    ▼
partOfSpeech = languageParser.GetPartOfSpeech("noun") → Id=1
    │
    ▼
nodes = languageParser.GetDeclesionNodes(1) → [Node{Id=1, Name="number"}]
    │
    ▼
ruleCombination = ","
    │
    ▼
For each node in nodes:
    │
    ├─► dimensionValue = word.Dimensions[node.Name] → "plural"
    │
    ├─► dimension = languageParser.GetDimensionNode(node.Name, posId, dimensionValue)
    │       → PolyglotDimensionNode{Id=2, Name="plural"}
    │
    ├─► ruleCombination += $"{dimension.Id}," → ",2,"
    │
    ▼
Final ruleCombination = ",2,"
```

## Generation rule matching flow

```
languageParser.GetDeclesionGenerationRules(",2,", 1)
    │
    ▼
language.DeclesionCollection.DeclesionGenerationRules.Where(x =>
    x.RuleCombination == ",2," &&
    x.PartOfSpeechId == 1)
    │
    ▼
Returns matching rules (can be multiple, ordered by XML/Index)
    │
    ▼
Example:
    PolyglotDeclesionGenerationRule{
        RuleCombination = ",2,",
        RegularExpression = "$",
        PartOfSpeechId = 1,
        Transformations = [{Pattern="$", Replacement="i"}]
    }
```

## Transformation application flow

```
DeclesionBuilder.GetWord(...)
    │
    ▼
baseWord = "fel" (from dictionary)
    │
    ▼
For each matching rule:
    │
    ├─► Regex.Matches("fel", "$") → Match (end of string)
    │
    ├─► For each transformation:
    │       │
    │       ▼
    │       Regex.Replace("fel", "$", "i") → "feli"
    │
    ▼
If multiple rules match, apply sequentially
    │
    ▼
If multiple transformations in rule, apply in order
    │
    ▼
Return final word: "feli"
```

## Multi-node rule combination example

For a part of speech with TWO declension nodes (e.g., noun with "number" and "case"):

```
Word{PartOfSpeechName="noun", Dimensions={"number":"plural", "case":"accusative"}}

nodes = GetDeclesionNodes(nounId) → [
    Node{Id=1, Name="number", Dimensions=[{Id=1,Name="singular"},{Id=2,Name="plural"}]},
    Node{Id=3, Name="case", Dimensions=[{Id=4,Name="nominative"},{Id=5,Name="accusative"}]}
]

ruleCombination building:
    ","
    → node "number": dimension "plural" → Id=2 → ",2,"
    → node "case": dimension "accusative" → Id=5 → ",2,5,"

Final ruleCombination = ",2,5,"

Matching rules must have RuleCombination=",2,5," AND PartOfSpeechId=nounId
```

## Sentence building flow

```
DeclesionBuilder.GetSentence("w1 w2 w3")
    │
    ▼
Split: ["w1", "w2", "w3"]
    │
    ▼
For each wordId:
    │
    ├─► wordRepository.Get(wordId) → Word
    │
    ├─► GetWord(Word) → constructedWord
    │
    ▼
Collect: ["fel", "feli", "cur"]
    │
    ▼
string.Join(' ', words) → "fel feli cur"
```

## Error propagation flow

```
Any step throws exception
    │
    ▼
Propagates up through call stack
    │
    ▼
LanguageValidator.Test() does not catch
    │
    ▼
Program.Main catches only AggregateException
    │
    ├─► If AggregateException: print inner messages, exit 1
    │
    └─► Other exceptions: unhandled → crash with stack trace
```

## File I/O flow

```
JsonRepository<Word>.GetAll() / Get(id)
    │
    ▼
NuciDAL.JsonRepository implementation
    │
    ▼
Reads JSON file from disk
    │
    ▼
Deserialises to List<Word> / Word
    │
    ▼
Returns to caller

JsonRepository<Sentence>.GetAll()
    │
    ▼
Same pattern for sentences
```