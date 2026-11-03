# Repository overview

## Purpose

Polyglot Language Validator is a command-line tool that validates constructed language outputs against expected results. It processes Polyglot dictionary XML files (containing parts of speech, lexicon, and declension rules), word JSON files (mapping local words to dimensions), and sentence JSON files (word-id sequences with expected constructed sentences). The tool builds sentences from word IDs, applies declension/generation rules from the dictionary, and compares generated output with expected output.

## Scope

The repository contains:

- **PolyglotLanguageValidator** — Main console application (net10.0)
- **PolyglotLanguageValidator.UnitTests** — Unit test project using NUnit 4.x and Moq

## Entry points

- `Program.Main` — CLI entry point, parses arguments, builds DI container, runs validation
- `ILanguageValidator.Test` — Core validation orchestration

## Principal capabilities

1. Parse Polyglot dictionary XML (parts of speech, lexicon, declension nodes, generation rules)
2. Load words JSON (local word, part of speech, dimensions)
3. Load sentences JSON (word-id sequences, expected constructed sentence)
4. Apply declension rules via regex transformations
5. Validate generated sentences against expected output
6. Report mismatches with detailed error messages
7. Exit with non-zero code on validation failure

## Component relationships

```
Program.Main
  → InputSettings (argument parsing)
  → ServiceCollection (DI configuration)
  → ILanguageValidator (LanguageValidator)
      → IDeclesionBuilder (DeclesionBuilder)
          → ILanguageParser (LanguageParser)
          → IRepository<Word> (JsonRepository)
      → IRepository<Sentence> (JsonRepository)
```

## Runtime topology

Single-process console application. No external services, databases, or network calls. File I/O only for input files.

## Major state

- Input file paths (dictionary, words, sentences)
- Parsed dictionary model (PolyglotDictionary)
- Loaded word and sentence collections
- Validation results (mismatches)

## Major integrations

- **NuciCLI.Arguments** — Command-line argument parsing
- **NuciDAL** — JSON file repository abstraction (IRepository, JsonRepository)
- **Microsoft.Extensions.DependencyInjection** — DI container
- **System.Xml.Serialization** — Dictionary XML deserialization
- **System.Text.RegularExpressions** — Declension rule transformations

## System-wide invariants

- Dictionary XML must conform to expected schema (partsOfSpeech, lexicon, declensionCollection)
- Words JSON must contain Word objects with Id, LocalWord, PartOfSpeechName, Dimensions
- Sentences JSON must contain Sentence objects with Id, Words (space-separated IDs), ConstructedLanguageSentence
- Declension rules are applied in order of RuleCombination match
- Validation fails if any sentence mismatches; all mismatches are reported together