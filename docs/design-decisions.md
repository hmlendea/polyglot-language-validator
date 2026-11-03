# Design decisions

## 1. Clean layered architecture with DI

**Decision**: Separate concerns into Presentation → Application Services → Domain Models → External Dependencies, wired via `Microsoft.Extensions.DependencyInjection`.

**Rationale**:
- Testability: Each layer can be mocked independently
- Extensibility: Swap implementations (e.g., different dictionary formats)
- Maintainability: Clear boundaries, single responsibility

**Alternatives considered**:
- Monolithic `Program.cs` — rejected (hard to test)
- MediatR/CQRS — rejected (overkill for CLI tool)

## 2. Interface-based service contracts

**Decision**: Define `ILanguageValidator`, `IDeclesionBuilder`, `ILanguageParser` interfaces; register singletons in DI.

**Rationale**:
- Enables unit testing with mocks
- Allows alternative implementations (e.g., database-backed parser)
- Explicit contracts document expected behaviour

## 3. XML dictionary with regex-based rule engine

**Decision**: Dictionary stored as XML (`PolyglotDictionary`); declension rules use regex guard + sequential transformations.

**Rationale**:
- Polyglot format is established in conlang community
- Regex provides flexible pattern matching for morphological rules
- Sequential application models real linguistic rule ordering

**Trade-offs**:
- Regex performance acceptable for CLI batch processing
- Not suitable for high-throughput real-time use

## 4. RuleCombination as comma-delimited dimension IDs

**Decision**: Encode applicable rules via `",dim1Id,dim2Id,..."` string matching XML order.

**Rationale**:
- Simple string equality for rule lookup (no complex query)
- Order encodes linguistic hierarchy (number before case, etc.)
- Human-readable in XML

## 5. JSON file repositories for words/sentences

**Decision**: Use `NuciDAL.JsonRepository<T>` for file-based storage.

**Rationale**:
- Zero-configuration persistence
- Human-editable input files
- No database dependency

**Trade-offs**:
- Not concurrent-safe (single-user CLI)
- Full file load on startup (acceptable for expected data sizes)

## 6. AggregateException for validation failures

**Decision**: Collect all `MismatchingValueException` and throw single `AggregateException`.

**Rationale**:
- Report all failures in one run (fail-fast would hide subsequent errors)
- Standard .NET pattern for multiple failures
- Exit code 1 signals failure to CI/CD

## 7. Silent success, verbose failure

**Decision**: No output on success; one line per mismatch on failure.

**Rationale**:
- Unix philosophy: silence is success
- CI/CD friendly (non-zero exit = failure)
- Output parseable for reporting

## 8. No nullable reference types enabled

**Decision**: Projects don't have `<Nullable>enable</Nullable>`.

**Rationale**:
- Legacy codebase compatibility
- External NuciDAL/NuciCLI may not be annotated
- Low risk for internal CLI tool

**Future**: Enable when dependencies support it.

## 9. Project folder name typo preserved

**Decision**: Keep `PolyglotLangageValidator` (missing 'u').

**Rationale**:
- Breaking change for existing clones/builds
- Namespace is correct (`PolyglotLanguageValidator`)
- Cosmetic only

## 10. Single-threaded synchronous execution

**Decision**: No async/await, no parallelism.

**Rationale**:
- Simple mental model
- File I/O is fast enough for expected inputs
- No shared state to protect

## 11. NUnit + Moq for testing

**Decision**: NUnit 4.x constraint model with Moq for mocks.

**Rationale**:
- Team familiarity
- Rich assertion syntax
- Good VS Code integration via adapter

## 12. Release via external script

**Decision**: `release.sh` downloads and runs `hmlendea/dotnet-release-scripts`.

**Rationale**:
- Shared release logic across repositories
- Versioning, tagging, packaging centralised
- Reduces per-repo maintenance