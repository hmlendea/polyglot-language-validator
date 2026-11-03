# Components — Presentation

## Program

**Location:** `PolyglotLanguageValidator/Program.cs`

**Namespace:** `PolyglotLanguageValidator`

**Purpose:** Application entry point, CLI argument parsing, DI container composition, validation orchestration.

### Public API

```csharp
public static void Main(string[] args)
```

### Responsibilities

1. Parse command-line arguments via `InputSettings`
2. Build `ServiceProvider` with all service registrations
3. Resolve `ILanguageValidator` and invoke `Test()`
4. Catch `AggregateException`, print all inner exception messages, exit with code 1

### Dependencies

- `InputSettings` — Argument parsing
- `Microsoft.Extensions.DependencyInjection` — DI container
- `ILanguageValidator` — Validation service
- `IDeclesionBuilder` — Sentence/word building
- `ILanguageParser` — Dictionary parsing
- `IRepository<Word>` — Word data access
- `IRepository<Sentence>` — Sentence data access

### Service registration

```csharp
new ServiceCollection()
    .AddSingleton(inputSettings)
    .AddSingleton<IDeclesionBuilder, DeclesionBuilder>()
    .AddSingleton<ILanguageParser, LanguageParser>()
    .AddSingleton<ILanguageValidator, LanguageValidator>()
    .AddSingleton<IFileRepository<Word>>(x => new JsonRepository<Word>(inputSettings.WordsFilePath))
    .AddSingleton<IFileRepository<Sentence>>(x => new JsonRepository<Sentence>(inputSettings.SentencesFilePath))
    .BuildServiceProvider();
```

### Error handling

- Catches only `AggregateException` from validator
- Prints each inner exception message to stdout
- Exits with code 1 on validation failure
- No handling for other exceptions (crash with stack trace)

### Configuration

- Reads file paths from `InputSettings` (CLI arguments)
- No other configuration sources

## InputSettings

**Location:** `PolyglotLanguageValidator/Configuration/InputSettings.cs`

**Namespace:** `PolyglotLanguageValidator.Configuration`

**Purpose:** CLI argument parsing and normalisation.

### Public API

```csharp
public string PolyglotDictionaryFilePath { get; set; }
public string WordsFilePath { get; set; }
public string SentencesFilePath { get; set; }

public InputSettings(string[] args)
```

### Argument definitions

| Argument | Alias | Required | Description |
|---|---|---|---|
| `--dictionary` | `-d` | Yes | Path to Polyglot dictionary XML |
| `--words` | `-w` | Yes | Path to words JSON |
| `--sentences` | `-s` | No* | Path to sentences JSON |

*Optional by parser, but required for validation (validator iterates sentences repository)

### Implementation details

- Uses `NuciCLI.Arguments.ArgumentParser`
- Normalises short aliases (`-d`, `-w`, `-s`) to long form before parsing
- `SentencesArgumentName` has `defaultValue: null` — not required by parser
- Stores parsed values in auto-properties

### Validation

- No file existence validation in `InputSettings`
- File access errors surface later during repository operations

## MismatchingValueException

**Location:** `PolyglotLanguageValidator/Exceptions/MismatchingValueException.cs`

**Namespace:** `PolyglotLanguageValidator.Exceptions`

**Purpose:** Represents a single sentence validation mismatch.

### Public API

```csharp
public MismatchingValueException()
public MismatchingValueException(string actualValue, string expectedValue)
public MismatchingValueException(string actualValue, string expectedValue, Exception innerException)
```

### Message format

```
"The actual value '{0}' does not match the expected value '{1}'."
```

- `{0}` = actual (generated) sentence
- `{1}` = expected sentence from input

### Usage

- Thrown by `LanguageValidator` for each mismatching sentence
- Collected into `AggregateException` for batch reporting
- Includes both values for easy debugging