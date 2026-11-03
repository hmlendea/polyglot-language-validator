# Behaviour — Inspect, edit, delete

## Not applicable

Polyglot Language Validator is a **read-only CLI validation tool**. It does not provide:

- **Inspect** — No interactive REPL, no query interface, no browsing UI
- **Edit** — No modification of dictionary, words, or sentences at runtime
- **Delete** — No removal of entries, no cleanup operations

## Data flow (read-only)

```
Input files (XML/JSON) → Parse → Validate → Report → Exit
```

No write operations occur.

## Alternative workflows

### To inspect data

```bash
# View dictionary structure
cat dictionary.xml | xmllint --format -

# View words
cat words.json | jq '.'

# View sentences
cat sentences.json | jq '.'

# Run validation to see mismatches
dotnet run -- -d dictionary.xml -w words.json -s sentences.json
```

### To edit data

Edit source files directly:
- `dictionary.xml` — Any XML/text editor
- `words.json` — Any JSON/text editor
- `sentences.json` — Any JSON/text editor

Then re-run validation.

### To delete data

Remove entries from source files, then re-run validation.

## Programmatic access (for tooling)

If building tooling on top, use the internal interfaces:

```csharp
// Load dictionary
var parser = new LanguageParser(dictionaryPath);

// Query
var word = parser.GetWord("cat", 1);
var nodes = parser.GetDeclesionNodes(1);

// Load repositories
var wordRepo = new JsonRepository<Word>(wordsPath);
var sentenceRepo = new JsonRepository<Sentence>(sentencesPath);

// Query
var allWords = wordRepo.GetAll();
var sentence = sentenceRepo.Get("s1");
```

## No persistence layer

- No database
- No transaction support
- No audit trail
- No versioning

Input files are the source of truth; manage them with Git or your preferred VCS.