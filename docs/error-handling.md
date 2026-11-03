# Error handling

## Exception hierarchy

```
Exception
├── MismatchingValueException (domain)
└── AggregateException (validation orchestrator)
    └── Contains: MismatchingValueException[]
```

## MismatchingValueException

**Location**: `PolyglotLangageValidator/Exceptions/MismatchingValueException.cs`

**Purpose**: Single sentence validation failure.

**Constructors**:
```csharp
public MismatchingValueException()
public MismatchingValueException(string message)
public MismatchingValueException(string message, Exception innerException)
```

**Message format**:
```
The actual value '{actual}' does not match the expected value '{expected}'.
```

**Usage**: Thrown by `LanguageValidator` when a generated sentence doesn't match expected.

## AggregateException

**Source**: `System` (BCL)

**Purpose**: Collects all validation failures for single-run reporting.

**Thrown by**: `LanguageValidator.Test()` when any sentence mismatches.

**Handling**: Caught in `Program.cs`, each inner exception printed to stdout, exit code 1.

## Argument parsing errors

**Source**: `NuciCLI.Arguments` library

**Behaviour**: Library throws on missing required args or parse failures.

**Handling**: Uncaught → .NET prints stack trace to stderr, non-zero exit.

## File I/O errors

**Sources**: `File.ReadAllText`, `XmlSerializer`, `JsonSerializer`

**Exceptions**: `FileNotFoundException`, `XmlException`, `JsonException`, `IOException`

**Handling**: Uncaught → .NET prints stack trace to stderr.

## Null reference handling

- **No nullable enabled** — Null checks not enforced by compiler
- **Defensive checks** — `LanguageParser` uses `FirstOrDefault()` and null checks
- **Fail-fast** — Invalid dictionary structure throws during parsing (constructor)

## Validation flow error handling

```
Program.Main
    │
    ▼
InputSettings.ParseArgs() → throws on bad args
    │
    ▼
BuildServiceProvider() → throws on DI config
    │
    ▼
LanguageValidator.Test()
    │
    ├── For each sentence:
    │     DeclesionBuilder.GetSentence() → throws on missing words/rules
    │     Compare generated vs expected
    │     Mismatch → collect MismatchingValueException
    │
    ▼
Any mismatches? → throw AggregateException(mismatches)
    │
    ▼
Program.Main catch AggregateException
    │
    ├── Print each inner exception.Message to Console.WriteLine
    │
    ▼
Environment.Exit(1)
```

## Unhandled exception policy

- **No global try/catch** beyond `AggregateException`
- **Fail fast** — Let .NET runtime handle unexpected errors
- **Stack traces** — Full traces on stderr for debugging

## Exit codes summary

| Code | Scenario |
|---|---|
| 0 | All validations passed |
| 1 | Validation failures (AggregateException caught) |
| 2+ | Unhandled exception (CLR default) |

## Best practices for extenders

1. **Throw `MismatchingValueException`** for domain validation failures
2. **Wrap multiple failures** in `AggregateException` if collecting
3. **Don't catch `Exception`** broadly — let unexpected errors surface
4. **Use `ArgumentNullException`** for null contract violations in public APIs
5. **Preserve stack traces** — `throw;` not `throw ex;`