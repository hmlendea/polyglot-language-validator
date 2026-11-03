# Repository structure

## Root layout

```
polyglot-language-validator/
├── LICENSE
├── PolyglotLanguageValidator.slnx
├── README.md
├── release.sh
├── PolyglotLangageValidator/          # Main application
│   ├── PolyglotLangageValidator.csproj
│   ├── Program.cs
│   ├── Configuration/
│   │   └── InputSettings.cs
│   ├── Exceptions/
│   │   └── MismatchingValueException.cs
│   ├── Models/
│   │   ├── Sentence.cs
│   │   ├── Word.cs
│   │   └── Polyglot/
│   │       ├── PolyglotClass.cs
│   │       ├── PolyglotDeclesionCollection.cs
│   │       ├── PolyglotDeclesionGenerationRule.cs
│   │       ├── PolyglotDeclesionGenerationRuleTransformation.cs
│   │       ├── PolyglotDeclesionNode.cs
│   │       ├── PolyglotDictionary.cs
│   │       ├── PolyglotDimensionNode.cs
│   │       ├── PolyglotLexicon.cs
│   │       ├── PolyglotPartsOfSpeech.cs
│   │       └── PolyglotWord.cs
│   ├── Service/
│   │   ├── DeclesionBuilder.cs
│   │   ├── IDeclesionBuilder.cs
│   │   ├── ILanguageParser.cs
///   │   ├── ILanguageValidator.cs
///   │   ├── LanguageParser.cs
///   │   └── LanguageValidator.cs
///   ├── bin/
///   └── obj/
/// ├── PolyglotLanguageValidator.UnitTests/
///   ├── PolyglotLanguageValidator.UnitTests.csproj
///   ├── Configuration/
///   │   └── InputSettingsTests.cs
///   ├── Exceptions/
///   │   └── MismatchingValueExceptionTests.cs
///   ├── Service/
///   │   ├── DeclesionBuilderTests.cs
///   │   ├── LanguageParserTests.cs
///   │   └── LanguageValidatorTests.cs
///   ├── bin/
///   └── obj/
/// ├── bin/
/// └── obj/
```

## Project organisation

### PolyglotLangageValidator

Main console application. Note: project folder name contains a typo ("Langage" instead of "Language"), preserved for historical reasons.

- **Configuration/** — CLI argument parsing and settings
- **Exceptions/** — Domain-specific exception types
- **Models/** — Data models
  - **Polyglot/** — XML-serialised dictionary models
- **Service/** — Business logic (parser, builder, validator)

### PolyglotLanguageValidator.UnitTests

Unit test project mirroring the main project structure:

- **Configuration/** — InputSettings tests
- **Exceptions/** — MismatchingValueException tests
- **Service/** — Service layer tests

## Namespace mapping

| Folder | Namespace |
|---|---|
| `PolyglotLangageValidator/` | `PolyglotLanguageValidator` |
| `PolyglotLangageValidator/Configuration/` | `PolyglotLanguageValidator.Configuration` |
| `PolyglotLangageValidator/Exceptions/` | `PolyglotLanguageValidator.Exceptions` |
| `PolyglotLangageValidator/Models/` | `PolyglotLanguageValidator.Models` |
| `PolyglotLangageValidator/Models/Polyglot/` | `PolyglotLanguageValidator.Models.Polyglot` |
| `PolyglotLangageValidator/Service/` | `PolyglotLanguageValidator.Service` |
| `PolyglotLanguageValidator.UnitTests/` | `PolyglotLanguageValidator.UnitTests` |

## File naming conventions

- Interfaces prefixed with `I` (e.g., `ILanguageParser`)
- Implementation classes without prefix (e.g., `LanguageParser`)
- Test classes suffixed with `Tests` (e.g., `LanguageParserTests`)
- One type per file, file name matches type name

## Build outputs

- `PolyglotLangageValidator/bin/Debug/net10.0/` — Application binaries
- `PolyglotLanguageValidator.UnitTests/bin/Debug/net10.0/` — Test binaries
- Root `bin/` and `obj/` — Solution-level build artifacts