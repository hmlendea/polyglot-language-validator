[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)  [![Latest Release](https://img.shields.io/github/v/release/hmlendea/polyglot-language-validator)](https://github.com/hmlendea/polyglot-language-validator/releases/latest) [![Build Status](https://github.com/hmlendea/polyglot-language-validator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/polyglot-language-validator/actions/workflows/dotnet.yml)

# Polyglot Language Validator

Polyglot Language Validator checks whether generated words and sentences in a constructed language match your expected outputs.

It takes:
- A Polyglot dictionary XML file (parts of speech, lexicon, declension rules)
- A words JSON file (local word + dimensions)
- A sentences JSON file (word-id sequences + expected constructed sentence)

Then it:
1. Builds each expected sentence from word IDs.
2. Applies dictionary declension/generation rules.
3. Compares generated output with expected output.
4. Prints mismatches and exits with non-zero status if any test fails.


## Build

```bash
dotnet build PolyglotTester.csproj
```

## Run

```bash
dotnet run --project PolyglotTester.csproj -- \
	--dictionary ./data/dictionary.xml \
	--words ./data/words.json \
	--sentences ./data/sentences.json
```

Short aliases are also supported:
- `-d` for `--dictionary`
- `-w` for `--words`
- `-s` for `--sentences`

Example:

```bash
dotnet run --project PolyglotTester.csproj -- \
	-d ./data/dictionary.xml \
	-w ./data/words.json \
	-s ./data/sentences.json
```

## CLI Arguments

| Argument | Alias | Required | Description |
|---|---|---|---|
| `--dictionary` | `-d` | Yes | Path to the Polyglot dictionary XML |
| `--words` | `-w` | Yes | Path to words JSON |
| `--sentences` | `-s` | Optional by parser, required for validation | Path to sentences JSON |

Note: The app always performs sentence validation at runtime, so provide `--sentences` for normal usage.

## Input Formats

### 1) Dictionary XML

Root element must be `<dictionary>` with these sections:
- `<partsOfSpeech>` containing `<class>` entries
- `<lexicon>` containing `<word>` entries
- `<declensionCollection>` containing:
	- `<declensionNode>` entries (dimensions)
	- `<decGenRule>` entries (regex-based transformations)

Minimal example:

```xml
<dictionary>
	<partsOfSpeech>
		<class>
			<classId>1</classId>
			<className>noun</className>
		</class>
	</partsOfSpeech>

	<lexicon>
		<word>
			<wordId>1</wordId>
			<localWord>cat</localWord>
			<conWord>fel</conWord>
			<wordTypeId>1</wordTypeId>
		</word>
	</lexicon>

	<declensionCollection>
		<declensionNode>
			<declensionId>1</declensionId>
			<declensionText>number</declensionText>
			<declensionRelatedId>1</declensionRelatedId>
			<dimensionNode>
				<dimensionId>1</dimensionId>
				<dimensionName>singular</dimensionName>
			</dimensionNode>
			<dimensionNode>
				<dimensionId>2</dimensionId>
				<dimensionName>plural</dimensionName>
			</dimensionNode>
		</declensionNode>

		<decGenRule>
			<decGenRuleComb>,2,</decGenRuleComb>
			<decGenRuleRegex>$</decGenRuleRegex>
			<decGenRuleName>plural-suffix</decGenRuleName>
			<decGenRuleTypeId>1</decGenRuleTypeId>
			<decGenRuleIndex>1</decGenRuleIndex>
			<decGenTrans>
				<decGenTransRegex>$</decGenTransRegex>
				<decGenTransReplace>i</decGenTransReplace>
			</decGenTrans>
		</decGenRule>
	</declensionCollection>
</dictionary>
```

### 2) Words JSON

Each entry describes one local word and its grammatical dimensions.

```json
[
	{
		"id": "w1",
		"localWord": "cat",
		"partOfSpeechName": "noun",
		"dimensions": {
			"number": "singular"
		}
	},
	{
		"id": "w2",
		"localWord": "cat",
		"partOfSpeechName": "noun",
		"dimensions": {
			"number": "plural"
		}
	}
]
```

### 3) Sentences JSON

`words` is a space-separated list of word IDs from the words JSON.

```json
[
	{
		"id": "s1",
		"words": "w1 w2",
		"constructedLanguageSentence": "fel feli",
		"localLanguageSentence": "cat cats"
	}
]
```

## Validation Behavior

For each sentence:
1. Split `words` by spaces.
2. Resolve each ID from the words repository.
3. Generate each constructed word from dictionary lexicon + declension rules.
4. Join generated words with spaces.
5. Compare against `constructedLanguageSentence`.

If mismatches exist, the app prints one error per mismatch, for example:

```text
The actual value 'feli fel' does not match the expected value 'fel feli'.
```

Exit codes:
- `0`: all sentences matched
- `1`: at least one mismatch (or aggregate validation error)

## Troubleshooting

- No word found in dictionary:
	- Ensure `partOfSpeechName` exists in `<partsOfSpeech>`.
	- Ensure `localWord` appears in `<lexicon>` for the same part of speech.
- Dimension or rule lookup fails:
	- Ensure all required dimension names exist in each word's `dimensions` object.
	- Ensure rule combinations (`decGenRuleComb`) match your dimension IDs ordering.
- Missing sentence data:
	- Provide `--sentences` with valid JSON input.

## Target Framework

The project currently targets `net10.0`.

## Development

### Prerequisites

- .NET SDK compatible with the target framework

### Build

```bash
dotnet build PolyglotLanguageValidator.csproj
```

### Run

```bash
dotnet run --project PolyglotLanguageValidator.csproj
```

### Test

Run the NUnit unit-test project with:

```bash
 dotnet test PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj
```

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/polyglot-language-validator/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🏗️ Architecture

Polyglot Language Validator follows a layered architecture with dependency injection and repository pattern. The system separates concerns across entry, validation, building, parsing, data access, and model layers.

See the [Architecture](./ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## Documentation

- [Documentation index](docs/INDEX.md) — Complete documentation catalogue

## Related Projects

- [Polyglot Language Validator](https://github.com/hmlendea/polyglot-language-validator) for the validator utility
- [Polyglot Language Validate](https://github.com/hmlendea/polyglot-language-validate) for the GitHub Action

## 🛡️ Privacy and Data

Polyglot Language Validator processes all data locally on the user's machine with no network communication, telemetry, or external data transfers. See [PRIVACY.md](./PRIVACY.md) for details.

## 🔒 Security

Polyglot Language Validator runs locally with no network communication or external attack surface. See [SECURITY.md](./SECURITY.md) for vulnerability reporting and supported versions.

## License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.
