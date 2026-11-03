# Quick start

## Prerequisites

- .NET 10.0 SDK installed
- Git (optional, for cloning)

## Clone and build

```bash
git clone <repository-url>
cd polyglot-language-validator
dotnet build PolyglotLanguageValidator.slnx
```

## Prepare input files

You need three files:

### 1. Dictionary (XML) — `dictionary.xml`

```xml
<?xml version="1.0" encoding="utf-8"?>
<PolyglotDictionary>
  <PartsOfSpeech>
    <PolyglotClass Id="1" Name="noun" />
    <PolyglotClass Id="2" Name="verb" />
  </PartsOfSpeech>
  <Lexicon>
    <PolyglotWord WordTypeId="1" LocalWord="cat" ConstructedWord="fel" />
    <PolyglotWord WordTypeId="2" LocalWord="run" ConstructedWord="cur" />
  </Lexicon>
  <DeclesionCollection>
    <DeclesionNodes>
      <DeclesionNode DeclensionId="1" DeclensionText="number" DeclensionRelatedId="1">
        <DimensionNode DimensionId="1" DimensionName="singular" />
        <DimensionNode DimensionId="2" DimensionName="plural" />
      </DeclesionNode>
    </DeclesionNodes>
    <DeclesionGenerationRules>
      <DeclesionGenerationRule DecGenRuleComb=",1," DecGenRuleRegex="$" DecGenRuleName="singular" DecGenRuleTypeId="1" DecGenRuleIndex="1" />
      <DeclesionGenerationRule DecGenRuleComb=",2," DecGenRuleRegex="$" DecGenRuleName="plural" DecGenRuleTypeId="1" DecGenRuleIndex="1">
        <DeclesionGenerationRuleTransformation DecGenTransRegex="$" DecGenTransReplace="i" />
      </DeclesionGenerationRule>
    </DeclesionGenerationRules>
  </DeclesionCollection>
</PolyglotDictionary>
```

### 2. Words (JSON) — `words.json`

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
    "LocalWord": "cat",
    "PartOfSpeechName": "noun",
    "Dimensions": { "number": "plural" }
  }
]
```

### 3. Sentences (JSON) — `sentences.json`

```json
[
  {
    "Id": "s1",
    "Words": "w1",
    "ConstructedLanguageSentence": "fel",
    "LocalLanguageSentence": "cat"
  },
  {
    "Id": "s2",
    "Words": "w2",
    "ConstructedLanguageSentence": "feli",
    "LocalLanguageSentence": "cats"
  }
]
```

## Run validation

```bash
dotnet run --project PolyglotLangageValidator/PolyglotLangageValidator.csproj -- \
  --dictionary dictionary.xml \
  --words words.json \
  --sentences sentences.json
```

## Expected output

- **Success** (exit code 0): No output
- **Failure** (exit code 1): Mismatch lines printed

## Run tests

```bash
dotnet test PolyglotLanguageValidator.slnx
```

## Next steps

- [Configuration](configuration.md) — All CLI options
- [API usage examples](api-usage-examples.md) — More complex examples
- [Architecture](architecture.md) — How it works internally