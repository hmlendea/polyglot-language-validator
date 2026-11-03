# State and persistence

## State model

**No runtime state persistence.** The application is stateless between runs.

## Data sources (read-only at runtime)

| Source | Format | Loaded by | Lifetime |
|---|---|---|---|
| Dictionary | XML | `LanguageParser` constructor | Process |
| Words | JSON | `JsonRepository<Word>` | Per call |
| Sentences | JSON | `JsonRepository<Sentence>` | Per call |

## In-memory state

### LanguageParser

- `PolyglotDictionary` object graph (deserialized once)
- Collections: `PartsOfSpeech`, `Lexicon`, `DeclesionCollection`
- **Immutable after construction** — no modifications

### Repositories (NuciDAL.JsonRepository)

- No in-memory cache
- `GetAll()` → reads file, deserializes, returns
- `Get(id)` → reads file, deserializes, finds, returns
- File read on **every call**

### Services

- `LanguageValidator` — stateless orchestrator
- `DeclesionBuilder` — stateless transformer
- `InputSettings` — holds parsed CLI args (immutable after parse)

## Persistence

### None

- No database
- No local storage (SQLite, files, etc.)
- No cloud storage
- No caching layer

### Input files

- Treated as **source of truth**
- Not modified by application
- User manages externally

### Output

- Stdout only (validation failures)
- No log files
- No result files
- No checkpointing

## File I/O patterns

### Dictionary (XML)

```csharp
// LanguageParser constructor
using var stream = File.OpenRead(dictionaryPath);
dictionary = (PolyglotDictionary)serializer.Deserialize(stream);
```
- **Once** at startup
- Stream closed immediately
- Full object graph in memory

### Words/Sentences (JSON)

```csharp
// JsonRepository<T>.GetAll()
var json = File.ReadAllText(filePath);
return JsonSerializer.Deserialize<List<T>>(json);
```
- **Every call** to `GetAll()` or `Get(id)`
- No pooling, no caching

## Concurrency implications

- **Not thread-safe** for concurrent file writes (not applicable — read-only)
- Multiple processes reading same files → safe
- Multiple processes writing → not supported

## Backup/recovery

- **Not applicable** — no state to backup
- Input files are user-managed
- Version control input files for history

## Migration

- **No migration needed** — no schema versioning
- Dictionary XML changes → update input file
- Words/Sentences JSON changes → update input files
- Code handles missing/extra fields gracefully (JSON serializer defaults)

## Capacity

- Limited by available memory (dictionary + words + sentences)
- No streaming for large files — full deserialization
- Expected use: thousands of entries, not millions