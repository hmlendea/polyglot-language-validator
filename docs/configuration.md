# Configuration

## CLI arguments

All configuration is via command-line arguments. No configuration files, environment variables, or appsettings.json.

### Required arguments

| Argument | Alias | Description |
|---|---|---|
| `--dictionary` | `-d` | Path to Polyglot dictionary XML file |
| `--words` | `-w` | Path to words JSON file |

### Optional arguments

| Argument | Alias | Description |
|---|---|---|
| `--sentences` | `-s` | Path to sentences JSON file |

### Argument parsing

- Uses `NuciCLI.Arguments` library
- Case-insensitive argument names
- Values can be separated by space or equals: `--dictionary file.xml` or `--dictionary=file.xml`
- Paths can be relative or absolute

## Input file formats

### Dictionary (XML)

Must conform to `PolyglotDictionary` schema:

```xml
<PolyglotDictionary>
  <PartsOfSpeech>
    <PolyglotClass Id="int" Name="string" />
  </PartsOfSpeech>
  <Lexicon>
    <PolyglotWord WordTypeId="int" LocalWord="string" ConstructedWord="string" />
  </Lexicon>
  <DeclesionCollection>
    <DeclesionNodes>
      <DeclesionNode DeclensionId="int" DeclensionText="string" DeclensionRelatedId="int">
        <DimensionNode DimensionId="int" DimensionName="string" />
      </DeclesionNode>
    </DeclesionNodes>
    <DeclesionGenerationRules>
      <DeclesionGenerationRule
        DecGenRuleComb="string"
        DecGenRuleRegex="string"
        DecGenRuleName="string"
        DecGenRuleTypeId="int"
        DecGenRuleIndex="int">
        <DeclesionGenerationRuleTransformation
          DecGenTransRegex="string"
          DecGenTransReplace="string" />
      </DeclesionGenerationRule>
    </DeclesionGenerationRules>
  </DeclesionCollection>
</PolyglotDictionary>
```

### Words (JSON)

Array of `Word` objects:

```json
[
  {
    "Id": "string",
    "LocalWord": "string",
    "PartOfSpeechName": "string",
    "Dimensions": { "string": "string" }
  }
]
```

- `Id` — Unique identifier (referenced by sentences)
- `LocalWord` — Source language word (matches dictionary `LocalWord`)
- `PartOfSpeechName` — Must match a `PolyglotClass.Name` in dictionary
- `Dimensions` — Key-value pairs; keys must match `DeclesionNode.DeclensionText` values

### Sentences (JSON)

Array of `Sentence` objects:

```json
[
  {
    "Id": "string",
    "Words": "string",
    "ConstructedLanguageSentence": "string",
    "LocalLanguageSentence": "string"
  }
]
```

- `Id` — Unique identifier
- `Words` — Space-separated word IDs (e.g., `"w1 w2 w3"`)
- `ConstructedLanguageSentence` — Expected output after declension
- `LocalLanguageSentence` — Source language sentence (for reference only)

## Environment variables

None used.

## Runtime configuration

None. No `appsettings.json`, no `IConfiguration`, no options pattern.

## Validation rules

1. Dictionary file must exist and be valid XML
2. Words file must exist and be valid JSON
3. Sentences file is optional for parsing but required for validation (validator iterates sentences repository)
4. All word IDs referenced in sentences must exist in words file
5. All `PartOfSpeechName` values must exist in dictionary
6. All dimension keys must match declension node names for that part of speech
7. All dimension values must exist as dimension nodes for the corresponding declension node

## Exit codes

| Code | Condition |
|---|---|
| 0 | All sentences validated successfully |
| 1 | One or more mismatches (details on stdout) |
| Other | Unhandled exception (stack trace on stderr) |