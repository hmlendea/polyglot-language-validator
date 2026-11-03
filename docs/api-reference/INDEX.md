# API reference

## Overview

Polyglot Language Validator is a **command-line application**, not a web API. It does not expose HTTP endpoints, REST APIs, or gRPC services.

The "API" consists of:
1. **CLI interface** — Command-line arguments
2. **Internal service interfaces** — For extensibility via dependency injection

## CLI interface

### Command

```bash
dotnet run --project PolyglotLangageValidator.csproj -- \
    --dictionary <path> \
    --words <path> \
    --sentences <path>
```

### Arguments

| Argument | Alias | Required | Description |
|---|---|---|---|
| `--dictionary` | `-d` | Yes | Path to Polyglot dictionary XML file |
| `--words` | `-w` | Yes | Path to words JSON file |
| `--sentences` | `-s` | No* | Path to sentences JSON file |

*Optional by parser, but required for validation to execute (validator iterates sentences repository)

### Exit codes

| Code | Meaning |
|---|---|
| `0` | All sentences validated successfully |
| `1` | One or more sentence mismatches (details printed to stdout) |
| Other | Unhandled exception (stack trace printed) |

### Output

- **Success**: No output (silent)
- **Failure**: One line per mismatch:
  ```
  The actual value 'generated sentence' does not match the expected value 'expected sentence'.
  ```

## Internal service interfaces

These interfaces enable custom implementations via DI replacement in `Program.BuildServiceProvider()`.

### ILanguageValidator

```csharp
namespace PolyglotLanguageValidator.Service
{
    public interface ILanguageValidator
    {
        void Test();
    }
}
```

**Implementation:** `LanguageValidator`

**Purpose:** Orchestrates full validation process.

### IDeclesionBuilder

```csharp
namespace PolyglotLanguageValidator.Service
{
    public interface IDeclesionBuilder
    {
        string GetWord(Word word);
        string GetSentence(string sentence);
        string GetSentence(params string[] words);
    }
}
```

**Implementation:** `DeclesionBuilder`

**Purpose:** Applies declension rules to words and builds sentences.

### ILanguageParser

```csharp
namespace PolyglotLanguageValidator.Service
{
    public interface ILanguageParser
    {
        string GetWord(string localWord, int partOfSpeechId);
        PolyglotClass GetPartOfSpeech(int id);
        PolyglotClass GetPartOfSpeech(string name);
        IEnumerable<PolyglotDeclesionNode> GetDeclesionNodes(int partOfSpeechId);
        PolyglotDeclesionNode GetDeclesionNode(string name, int partOfSpeechId);
        PolyglotDimensionNode GetDimensionNode(string declesionNodeName, int partOfSpeechId, string dimensionValue);
        IEnumerable<PolyglotDeclesionGenerationRule> GetDeclesionGenerationRules(string ruleCombination, int partOfSpeechId);
    }
}
```

**Implementation:** `LanguageParser`

**Purpose:** Loads dictionary XML and provides typed lookups.

### IRepository<T> (from NuciDAL)

```csharp
namespace NuciDAL.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        T Get(string id);
        IEnumerable<T> GetAll();
        // ... other CRUD methods not used by validator
    }
}
```

**Implementation:** `JsonRepository<T>`

**Purpose:** File-based JSON storage for `Word` and `Sentence` entities.

## Extension points

To customise behaviour, replace service registrations in `Program.BuildServiceProvider()`:

```csharp
// Custom dictionary format
.AddSingleton<ILanguageParser, CustomLanguageParser>()

// Custom rule engine
.AddSingleton<IDeclesionBuilder, CustomDeclesionBuilder>()

// Custom storage (database, API, etc.)
.AddSingleton<IFileRepository<Word>, CustomWordRepository>()
.AddSingleton<IFileRepository<Sentence>, CustomSentenceRepository>()

// Custom validation logic
.AddSingleton<ILanguageValidator, CustomLanguageValidator>()
```

All custom implementations must adhere to the interface contracts documented above.