# Testing

## Test structure

```
PolyglotLanguageValidator.UnitTests/
├── Configuration/
│   └── InputSettingsTests.cs
├── Exceptions/
│   └── MismatchingValueExceptionTests.cs
└── Service/
    ├── DeclesionBuilderTests.cs
    ├── LanguageParserTests.cs
    └── LanguageValidatorTests.cs
```

## Test framework

- **NUnit 4.3.2** — Test framework
- **Moq 4.20.72** — Mocking
- **NUnit3TestAdapter 5.0.0** — VS Code / CLI integration
- **Microsoft.NET.Test.Sdk 17.13.0** — Test host

## Run tests

```bash
# All tests
dotnet test PolyglotLanguageValidator.slnx

# Specific project
dotnet test PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj

# Verbose
dotnet test --logger "console;verbosity=detailed"

# Filter by category/name
dotnet test --filter "FullyQualifiedName~LanguageParserTests"
```

## Test patterns

### Unit tests (isolated)

- Mock all dependencies (`ILanguageParser`, `IRepository<T>`, `IDeclesionBuilder`)
- Test single class behaviour
- Use NUnit constraint model: `Assert.That(actual, Is.EqualTo(expected))`

### Integration-style tests

- Use real `LanguageParser` with test dictionary XML
- Use real `JsonRepository<T>` with test JSON files
- Test end-to-end flows

## Test coverage by component

### LanguageParserTests.cs (7 tests)

| Test | Purpose |
|---|---|
| `GetWord_ValidInput_ReturnsConstructedWord` | Basic lexicon lookup |
| `GetWord_InvalidLocalWord_ReturnsNull` | Missing word handling |
| `GetPartOfSpeech_ById_ReturnsCorrectClass` | Part of speech by ID |
| `GetPartOfSpeech_ByName_ReturnsCorrectClass` | Part of speech by name |
| `GetDeclesionNodes_ReturnsNodesForPartOfSpeech` | Declension node retrieval |
| `GetDimensionNode_ReturnsCorrectDimension` | Dimension lookup |
| `GetDeclesionGenerationRules_ReturnsMatchingRules` | Rule filtering by combination |

### DeclesionBuilderTests.cs (6 tests)

| Test | Purpose |
|---|---|
| `GetWord_AppliesSingularRule_ReturnsBaseWord` | Singular (no transformation) |
| `GetWord_AppliesPluralRule_ReturnsMutatedWord` | Plural (with transformation) |
| `GetWord_MultipleRules_AppliesAllMatching` | Multiple rule application |
| `GetSentence_JoinsWordsWithSpaces` | Sentence building |
| `GetSentence_Params_JoinsWordsWithSpaces` | Params overload |
| `GetWord_NoMatchingRules_ReturnsBaseWord` | No rules edge case |

### LanguageValidatorTests.cs (4 tests)

| Test | Purpose |
|---|---|
| `Test_AllSentencesMatch_DoesNotThrow` | Success case |
| `Test_OneSentenceMismatch_ThrowsAggregateException` | Single failure |
| `Test_MultipleSentenceMismatches_ThrowsAggregateExceptionWithAll` | Multiple failures |
| `Test_NoSentences_DoesNotThrow` | Empty repository |

### InputSettingsTests.cs

- Argument parsing validation
- Required/optional argument handling
- Alias verification

### MismatchingValueExceptionTests.cs

- Exception message formatting
- Constructor overloads
- Inner exception preservation

## Mocking patterns

```csharp
// Mock repository
var wordRepo = new Mock<IFileRepository<Word>>();
wordRepo.Setup(r => r.GetAll()).Returns(words);

// Mock parser
var parser = new Mock<ILanguageParser>();
parser.Setup(p => p.GetWord("cat", 1)).Returns("fel");
parser.Setup(p => p.GetPartOfSpeech("noun")).Returns(new PolyglotClass { Id = 1, Name = "noun" });

// Mock builder
var builder = new Mock<IDeclesionBuilder>();
builder.Setup(b => b.GetWord(It.IsAny<Word>())).Returns("feli");
```

## Assertions

```csharp
// Equality
Assert.That(actual, Is.EqualTo(expected));

// Exception
Assert.That(() => validator.Test(), Throws.TypeOf<AggregateException>());

// Collection
Assert.That(exceptions, Has.Count.EqualTo(2));

// Message content
Assert.That(ex.Message, Does.Contain("expected value"));
```

## Test data

- Inline object creation (no external fixtures)
- Minimal dictionary XML strings in tests
- Minimal JSON arrays in tests
- No shared test context between test classes

## CI integration

- Runs in GitHub Actions on push/PR
- `dotnet test --no-build --verbosity normal`
- Fails build on any test failure
- No code coverage enforcement

## Adding tests

1. Create test class in appropriate folder
2. Name: `<ClassName>Tests.cs`
3. Use `[TestFixture]` attribute
4. Use `[Test]` attribute for test methods
5. Follow naming: `MethodName_Scenario_ExpectedResult`
6. Mock dependencies in `[SetUp]` or inline