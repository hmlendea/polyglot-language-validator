# Data model

## Domain entities

### Word

```csharp
public sealed class Word : EntityBase
{
    public string LocalWord { get; set; }
    public string PartOfSpeechName { get; set; }
    public Dictionary<string, string> Dimensions { get; set; }
}
```

- **Id** (from `EntityBase`) — Unique identifier, matches word ID in sentences
- **LocalWord** — Source language word (e.g., "cat", "cats")
- **PartOfSpeechName** — References `PolyglotClass.Name` (e.g., "noun", "verb")
- **Dimensions** — Key-value pairs mapping declension node names to dimension values (e.g., `{"number": "plural"}`)

### Sentence

```csharp
public sealed class Sentence : EntityBase
{
    public string Words { get; set; }
    public string ConstructedLanguageSentence { get; set; }
    public string LocalLanguageSentence { get; set; }
}
```

- **Id** (from `EntityBase`) — Unique identifier
- **Words** — Space-separated word IDs (e.g., "w1 w2 w3")
- **ConstructedLanguageSentence** — Expected output after declension (e.g., "feli cur")
- **LocalLanguageSentence** — Source language sentence (e.g., "cats run") — not used in validation

## Polyglot dictionary models (XML-serialised)

### PolyglotDictionary (root)

```csharp
[XmlRoot("dictionary")]
public sealed class PolyglotDictionary
{
    public PolyglotPartsOfSpeech PartsOfSpeech { get; set; }
    public PolyglotLexicon Lexicon { get; set; }
    public PolyglotDeclesionCollection DeclesionCollection { get; set; }
}
```

### PolyglotPartsOfSpeech

```csharp
public sealed class PolyglotPartsOfSpeech
{
    [XmlElement("class")]
    public List<PolyglotClass> Classes;
}
```

### PolyglotClass

```csharp
public sealed class PolyglotClass
{
    [XmlElement("classId")] public int Id { get; set; }
    [XmlElement("className")] public string Name { get; set; }
    [XmlElement("classNotes")] public string ClassNotes { get; set; }
    [XmlElement("definitionMandatoryClass")] public string DefinitionMandatoryClass { get; set; }
    [XmlElement("pronunciationMandatoryClass")] public string PronunciationMandatoryClass { get; set; }
    [XmlElement("classPattern")] public string ClassPattern { get; set; }
    [XmlElement("classGloss")] public string ClassGloss { get; set; }
}
```

- **Id** — Unique identifier, referenced by `PolyglotWord.WordTypeId` and `PolyglotDeclesionNode.PartOfSpeechId`
- **Name** — Human-readable name (e.g., "noun", "verb"), used by `Word.PartOfSpeechName`

### PolyglotLexicon

```csharp
public sealed class PolyglotLexicon
{
    [XmlElement("word")]
    public List<PolyglotWord> Words;
}
```

### PolyglotWord

```csharp
public sealed class PolyglotWord
{
    [XmlElement("wordId")] public int WordId { get; set; }
    [XmlElement("localWord")] public string LocalWord { get; set; }
    [XmlElement("conWord")] public string ConstructedWord { get; set; }
    [XmlElement("wordTypeId")] public int WordTypeId { get; set; }
}
```

- **WordId** — Internal dictionary ID (not used directly by validator)
- **LocalWord** — Comma-separated local word forms (e.g., "cat, cats")
- **ConstructedWord** — Base constructed form before declension (e.g., "fel")
- **WordTypeId** — References `PolyglotClass.Id`

### PolyglotDeclesionCollection

```csharp
public sealed class PolyglotDeclesionCollection
{
    [XmlElement("declensionNode")]
    public List<PolyglotDeclesionNode> Nodes;

    [XmlElement("decGenRule")]
    public List<PolyglotDeclesionGenerationRule> DeclesionGenerationRules;
}
```

### PolyglotDeclesionNode

```csharp
public sealed class PolyglotDeclesionNode
{
    [XmlElement("declensionId")] public int Id { get; set; }
    [XmlElement("declensionText")] public string Name { get; set; }
    [XmlElement("declensionRelatedId")] public int PartOfSpeechId { get; set; }
    [XmlElement("dimensionNode")]
    public List<PolyglotDimensionNode> Dimensions { get; set; }
}
```

- **Id** — Unique dimension identifier, used in `RuleCombination`
- **Name** — Dimension name (e.g., "number", "tense"), matches `Word.Dimensions` keys
- **PartOfSpeechId** — References `PolyglotClass.Id`
- **Dimensions** — Possible values for this dimension

### PolyglotDimensionNode

```csharp
public sealed class PolyglotDimensionNode
{
    [XmlElement("dimensionId")] public int Id { get; set; }
    [XmlElement("dimensionName")] public string Name { get; set; }
}
```

- **Id** — Used in `RuleCombination` string (e.g., ",1,2,")
- **Name** — Dimension value (e.g., "singular", "plural", "present")

### PolyglotDeclesionGenerationRule

```csharp
public sealed class PolyglotDeclesionGenerationRule
{
    [XmlElement("decGenRuleComb")] public string RuleCombination { get; set; }
    [XmlElement("decGenRuleRegex")] public string RegularExpression { get; set; }
    [XmlElement("decGenRuleName")] public string Name { get; set; }
    [XmlElement("decGenRuleTypeId")] public int PartOfSpeechId { get; set; }
    [XmlElement("decGenRuleIndex")] public int Index { get; set; }
    [XmlElement("decGenTrans")]
    public List<PolyglotDeclesionGenerationRuleTransformation> Transformations { get; set; }
}
```

- **RuleCombination** — Comma-delimited dimension IDs (e.g., ",1,2,") matching the sequence of declension nodes for a part of speech
- **RegularExpression** — Regex pattern that must match the constructed word for this rule to apply
- **PartOfSpeechId** — References `PolyglotClass.Id`
- **Transformations** — Ordered list of regex replacements to apply

### PolyglotDeclesionGenerationRuleTransformation

```csharp
public sealed class PolyglotDeclesionGenerationRuleTransformation
{
    [XmlElement("decGenTransRegex")] public string Pattern { get; set; }
    [XmlElement("decGenTransReplace")] public string Replacement { get; set; }
}
```

- **Pattern** — Regex pattern to match
- **Replacement** — Replacement string (can use capture groups)

## EntityBase

```csharp
// From NuciDAL.DataObjects
public abstract class EntityBase
{
    public string Id { get; set; }
}
```

Provides string `Id` for `Word` and `Sentence` entities.

## Relationships

```
PolyglotDictionary
  ├── PartsOfSpeech.Classes (PolyglotClass[])
  │       │
  │       ├── Id ◄── PolyglotWord.WordTypeId
  │       ├── Id ◄── PolyglotDeclesionNode.PartOfSpeechId
  │       └── Id ◄── PolyglotDeclesionGenerationRule.PartOfSpeechId
  │
  ├── Lexicon.Words (PolyglotWord[])
  │       └── WordTypeId ──► PolyglotClass.Id
  │
  └── DeclesionCollection
          ├── Nodes (PolyglotDeclesionNode[])
          │       ├── PartOfSpeechId ──► PolyglotClass.Id
          │       └── Dimensions (PolyglotDimensionNode[])
          │               └── Id used in RuleCombination
          │
          └── DeclesionGenerationRules (PolyglotDeclesionGenerationRule[])
                  ├── PartOfSpeechId ──► PolyglotClass.Id
                  ├── RuleCombination = "," + DimensionIds.Join(",") + ","
                  └── Transformations (PolyglotDeclesionGenerationRuleTransformation[])

Word (JSON)
  ├── Id ──► Sentence.Words (space-separated)
  ├── PartOfSpeechName ──► PolyglotClass.Name
  └── Dimensions[NodeName] = DimensionName ──► PolyglotDimensionNode.Name

Sentence (JSON)
  ├── Words = "w1 w2 w3" (Word IDs)
  └── ConstructedLanguageSentence = expected output
```

## RuleCombination format

The `RuleCombination` string encodes the selected dimension for each declension node in order:

```
Format: "," + dim1.Id + "," + dim2.Id + "," + ... + ","
Example: ",1,2,"  (first node → dimension ID 1, second node → dimension ID 2)
```

During validation, `DeclesionBuilder.GetWord` builds the rule combination by:
1. Getting all declension nodes for the word's part of speech (ordered by XML)
2. For each node, looking up the dimension specified in `Word.Dimensions[node.Name]`
3. Appending each dimension's `Id` to the combination string

## JSON input formats

### words.json

```json
[
  {
    "Id": "w1",
    "LocalWord": "cat",
    "PartOfSpeechName": "noun",
    "Dimensions": { "number": "singular" }
  },
  {
    "Id": "w2",
    "LocalWord": "cats",
    "PartOfSpeechName": "noun",
    "Dimensions": { "number": "plural" }
  }
]
```

### sentences.json

```json
[
  {
    "Id": "s1",
    "Words": "w1 w2",
    "ConstructedLanguageSentence": "fel feli",
    "LocalLanguageSentence": "cat cats"
  }
]
```

## Validation invariants

1. Every `Word.PartOfSpeechName` must exist in `PolyglotPartsOfSpeech.Classes`
2. Every `Word.Dimensions` key must match a `PolyglotDeclesionNode.Name` for that part of speech
3. Every `Word.Dimensions` value must match a `PolyglotDimensionNode.Name` under the corresponding node
4. `Sentence.Words` must reference existing `Word.Id` values
5. `RuleCombination` must exactly match the dimension ID sequence for the part of speech's declension nodes
6. `PolyglotDeclesionGenerationRule.RegularExpression` must match the base `ConstructedWord` for transformations to apply