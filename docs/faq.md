# FAQ

## General

### What is Polyglot Language Validator?

A CLI tool that validates constructed language (conlang) sentences against expected outputs using a Polyglot dictionary format.

### What problem does it solve?

Automates verification that your conlang's morphological rules (declensions, conjugations) produce the expected words and sentences.

### Is it a web service?

No. It's a standalone .NET console application.

## Usage

### What input files do I need?

1. **Dictionary (XML)** — Defines parts of speech, lexicon, declension nodes, and generation rules
2. **Words (JSON)** — Maps local words to parts of speech and grammatical dimensions
3. **Sentences (JSON)** — Expected constructed language sentences for validation

### Can I validate without sentences file?

The tool will run but `LanguageValidator.Test()` iterates the sentences repository, so no validation occurs without it.

### How do I specify multiple dimension values?

In words.json, the `Dimensions` object maps declension node names to dimension names:
```json
"Dimensions": { "number": "plural", "case": "accusative" }
```

### What's the RuleCombination format?

Comma-delimited dimension IDs with leading/trailing commas: `",1,2,"` — matches XML declension node order.

## Dictionary

### Where is the Polyglot dictionary schema documented?

See [configuration.md](configuration.md) for the XML structure.

### Can I have multiple rules for the same combination?

Yes. All rules with matching `RuleCombination` and `PartOfSpeechId` apply sequentially (XML order).

### How do capture groups work in transformations?

Use `$1`, `$2` in `DecGenTransReplace` to reference capture groups from `DecGenTransRegex`:
```xml
<DeclesionGenerationRuleTransformation DecGenTransRegex="(.+)s$" DecGenTransReplace="$1z" />
```

### What does the RegularExpression on a rule do?

Acts as a guard — rule only applies if regex matches the current word state (after previous transformations).

## Troubleshooting

### Why does my word not change?

- No matching generation rule for the RuleCombination
- Rule's RegularExpression doesn't match the word
- Transformation list is empty

### Why do I get "Sequence contains no matching element"?

Dictionary missing expected `PolyglotClass`, `PolyglotWord`, or `DimensionNode`.

### Can I debug rule application?

Add temporary `Console.WriteLine` in `DeclesionBuilder.GetWord` to print ruleCombination and matching rules.

## Development

### How do I run tests?

```bash
dotnet test PolyglotLanguageValidator.slnx
```

### How do I add a new part of speech?

1. Add `<PolyglotClass>` to dictionary XML
2. Add `<PolyglotWord>` entries with matching `WordTypeId`
3. Add `<DeclesionNode>` and `<DeclesionGenerationRule>` for its morphology

### Can I use a different dictionary format?

Yes — implement `ILanguageParser` and register in DI container.

### Can I use a database instead of JSON files?

Yes — implement `IFileRepository<Word>` and `IFileRepository<Sentence>` (from NuciDAL) and register in DI.

## Release

### How do I create a release?

```bash
./release.sh 1.2.3
```

### Where are releases published?

Configured in the shared `hmlendea/dotnet-release-scripts` — typically GitHub Releases and/or NuGet.

## Contributing

### Where do I start?

Read [quick-start.md](quick-start.md), then [architecture.md](architecture.md).

### What's the code style?

Standard .NET conventions. No `.editorconfig` enforced — follow existing patterns.

### Are there coding standards?

See [design-decisions.md](design-decisions.md) for architectural principles.