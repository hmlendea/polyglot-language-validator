# Architecture

## High-level architecture

Polyglot Language Validator follows a clean, layered architecture with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                      Program (CLI)                          │
│  - Argument parsing (InputSettings)                         │
│  - DI container setup                                       │
│  - Orchestration                                            │
└──────────────────────────┬──────────────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────────────┐
│                  ILanguageValidator                         │
│  LanguageValidator                                          │
│  - Iterates sentences                                       │
│  - Delegates to IDeclesionBuilder                           │
│  - Collects mismatches                                      │
│  - Throws AggregateException on failure                     │
└──────────────────────────┬──────────────────────────────────┘
                           │
           ┌───────────────┼───────────────┐
           ▼               ▼               ▼
┌──────────────────┐ ┌──────────────┐ ┌──────────────┐
│ IDeclesionBuilder│ │IRepository<  │ │IRepository<  │
│ DeclesionBuilder │ │Word>         │ │Sentence>     │
│                  │ │JsonRepository│ │JsonRepository│
└────────┬─────────┘ └──────────────┘ └──────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────┐
│                    ILanguageParser                          │
│  LanguageParser                                             │
│  - Loads & deserialises PolyglotDictionary XML              │
│  - Provides lookup methods for parts of speech,             │
│    declension nodes, dimensions, generation rules           │
└─────────────────────────────────────────────────────────────┘
```

## Layer responsibilities

| Layer | Components | Responsibility |
|---|---|---|
| **Entry** | `Program`, `InputSettings` | CLI parsing, DI composition, process lifecycle |
| **Validation** | `LanguageValidator`, `ILanguageValidator` | Orchestrate sentence validation, collect errors |
| **Building** | `DeclesionBuilder`, `IDeclesionBuilder` | Apply declension rules to words, build sentences |
| **Parsing** | `LanguageParser`, `ILanguageParser` | Load dictionary, provide typed lookups |
| **Data Access** | `JsonRepository<Word>`, `JsonRepository<Sentence>` | File I/O for JSON input files |
| **Models** | `Word`, `Sentence`, `Polyglot*` | Data transfer, XML/JSON serialisation |

## Dependency direction

```
Program
  → Configuration (InputSettings)
  → Service (interfaces & implementations)
  → Models (data objects)
  → External: NuciCLI, NuciDAL, Microsoft.Extensions.DI
```

No circular dependencies. All dependencies flow inward toward models.

## Key design patterns

### Dependency Injection
- Constructor injection throughout
- Interfaces for all services (`ILanguageValidator`, `IDeclesionBuilder`, `ILanguageParser`)
- `ServiceCollection` in `Program.BuildServiceProvider`

### Repository Pattern
- `IRepository<T>` from NuciDAL
- `JsonRepository<T>` implementation for file-based JSON storage
- Generic, reusable across `Word` and `Sentence`

### Strategy Pattern (implicit)
- `DeclesionGenerationRule` encapsulates transformation logic
- Rules selected by `RuleCombination` (dimension ID sequence)
- Multiple rules can match; all are applied in order

### Factory Pattern (implicit)
- `LanguageParser.LoadLanguage` creates `PolyglotDictionary` from XML
- `XmlSerializer` handles deserialisation

## Data flow

```
Input Files
    │
    ▼
┌─────────────────────────────────────────────────────────────┐
│  Dictionary XML ──► LanguageParser ──► PolyglotDictionary  │
│  Words JSON ─────► JsonRepository<Word> ──► Word[]         │
│  Sentences JSON ─► JsonRepository<Sentence> ──► Sentence[] │
└─────────────────────────────────────────────────────────────┘
    │
    ▼
LanguageValidator.Test()
    │
    ├─► For each Sentence:
    │     │
    │     ▼
    │  DeclesionBuilder.GetSentence(sentence.Words)
    │     │
    │     ├─► For each word ID:
    │     │     │
    │     │     ▼
    │     │  JsonRepository.Get(wordId) ──► Word
    │     │     │
    │     │     ▼
    │     │  DeclesionBuilder.GetWord(Word)
    │     │     │
    │     │     ├─► LanguageParser.GetPartOfSpeech(name)
    │     │     ├─► LanguageParser.GetWord(localWord, posId)
    │     │     ├─► LanguageParser.GetDeclesionNodes(posId)
    │     │     ├─► LanguageParser.GetDimensionNode(...)
    │     │     ├─► LanguageParser.GetDeclesionGenerationRules(...)
    │     │     │
    │     │     ▼
    │     │  Apply regex transformations
    │     │     │
    │     │     ▼
    │     │  Return constructed word
    │     │
    │     ▼
    │  Join words with spaces
    │
    ▼
Compare generated vs expected
    │
    ▼
Collect mismatches → AggregateException or success
```

## Configuration

- **InputSettings** — CLI arguments only (dictionary, words, sentences paths)
- No appsettings.json, environment variables, or config files
- Argument aliases: `-d`, `-w`, `-s`

## Error handling

- `MismatchingValueException` — Single sentence mismatch (actual vs expected)
- `AggregateException` — Collects all mismatches, thrown at end
- `Program.Main` catches `AggregateException`, prints all messages, exits 1
- No retry, fallback, or recovery logic

## Concurrency

- Single-threaded, synchronous execution
- No async/await, no parallelism
- No thread-safety concerns

## Extensibility points

1. **Custom repository** — Implement `IRepository<T>` for other storage
2. **Custom parser** — Implement `ILanguageParser` for other dictionary formats
3. **Custom builder** — Implement `IDeclesionBuilder` for different rule engines
4. **Custom validator** — Implement `ILanguageValidator` for different validation logic

All via DI registration in `Program.BuildServiceProvider`.