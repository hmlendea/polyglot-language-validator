# Behaviour — Browse and search

## Dictionary traversal

### Parts of speech

```csharp
// Get all parts of speech
var partsOfSpeech = parser.GetPartsOfSpeech(); // Not directly exposed, but via dictionary

// Get by ID
var noun = parser.GetPartOfSpeech(1);

// Get by name
var verb = parser.GetPartOfSpeech("verb");
```

### Lexicon lookup

```csharp
// Find constructed word for local word + part of speech
var constructed = parser.GetWord("cat", 1); // "fel"

// Matching: WordTypeId == partOfSpeechId AND LocalWord.Contains(localWord)
// Returns first match (XML order)
```

### Declension nodes

```csharp
// Get all declension nodes for a part of speech
var nodes = parser.GetDeclesionNodes(1); // number, case, etc.

// Get specific node by name
var numberNode = parser.GetDeclesionNode("number", 1);
```

### Dimension nodes

```csharp
// Get dimension value for a declension node
var singular = parser.GetDimensionNode("number", 1, "singular");
var plural = parser.GetDimensionNode("number", 1, "plural");
```

### Generation rules

```csharp
// Find rules for a rule combination + part of speech
var rules = parser.GetDeclesionGenerationRules(",1,", 1); // singular rules
var rules = parser.GetDeclesionGenerationRules(",2,", 1); // plural rules
```

## Word search

### By ID

```csharp
var word = wordRepository.Get("w1");
```

### All words

```csharp
var allWords = wordRepository.GetAll();
```

### Filter by part of speech

```csharp
var nouns = allWords.Where(w => w.PartOfSpeechName == "noun");
```

### Filter by dimension

```csharp
var pluralWords = allWords.Where(w => w.Dimensions.GetValueOrDefault("number") == "plural");
```

## Sentence search

### By ID

```csharp
var sentence = sentenceRepository.Get("s1");
```

### All sentences

```csharp
var allSentences = sentenceRepository.GetAll();
```

## Rule resolution search

### Build rule combination

```csharp
// From word dimensions and declension nodes
var ruleCombination = ",";
foreach (var node in parser.GetDeclesionNodes(partOfSpeechId)) {
    var dimValue = word.Dimensions[node.Name];
    var dimension = parser.GetDimensionNode(node.Name, partOfSpeechId, dimValue);
    ruleCombination += $"{dimension.Id},";
}
// Result: ",1,2," for number=singular, case=nominative
```

### Find matching rules

```csharp
var rules = parser.GetDeclesionGenerationRules(ruleCombination, partOfSpeechId);
// Returns all rules where DecGenRuleComb == ruleCombination AND DecGenRuleTypeId == partOfSpeechId
// Ordered by XML order (DecGenRuleIndex)
```

## Search patterns in codebase

### LanguageParser lookups (LINQ First/FirstOrDefault)

```csharp
// Parts of speech
dictionary.PartsOfSpeech.First(p => p.Id == id);
dictionary.PartsOfSpeech.First(p => p.Name == name);

// Lexicon
dictionary.Lexicon.First(w => w.WordTypeId == partOfSpeechId && w.LocalWord.Contains(localWord));

// Declension nodes
dictionary.DeclesionCollection.DeclesionNodes.Where(n => n.DeclensionRelatedId == partOfSpeechId);

// Dimension nodes
node.DimensionNodes.First(d => d.DimensionName == dimensionValue);

// Generation rules
dictionary.DeclesionCollection.DeclesionGenerationRules
    .Where(r => r.DecGenRuleComb == ruleCombination && r.DecGenRuleTypeId == partOfSpeechId)
    .OrderBy(r => r.DecGenRuleIndex);
```

### Repository access (NuciDAL)

```csharp
// JsonRepository<T> - file-based, no indexing
repository.GetAll(); // Reads file, deserializes, returns all
repository.Get(id);  // Reads file, deserializes, finds by Id
```

## Performance characteristics

- **Dictionary**: Loaded once, in-memory LINQ (O(n) scans)
- **Words/Sentences**: File read on every repository call
- **No indexes** — Linear search for all lookups
- **Expected scale**: Hundreds to low thousands of entries