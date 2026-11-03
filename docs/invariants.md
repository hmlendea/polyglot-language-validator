# Invariants

## System invariants

### 1. Dictionary immutability

- `LanguageParser` loads dictionary **once** in constructor
- Dictionary never reloaded or modified during execution
- All lookups read from in-memory collections

### 2. Rule application determinism

- Same inputs → same outputs (no randomness, no external state)
- Rules apply in **XML order** (stable sort)
- Transformations apply in **XML order** within each rule
- Regex matching uses .NET default options (case-sensitive, ECMAScript-compatible)

### 3. Validation completeness

- `LanguageValidator.Test()` iterates **all** sentences from repository
- Every sentence validated exactly once per run
- All mismatches collected before throwing `AggregateException`

### 4. Exit code contract

- `0` = all sentences match
- `1` = one or more mismatches
- Other = unhandled exception

### 5. Output contract

- **Success**: Zero bytes on stdout
- **Failure**: One line per mismatch on stdout, format fixed
- **Errors**: Stack traces on stderr only

### 6. File handling

- Input files read **once** at startup
- No file watching, no hot reload
- No temporary files created
- No output files written

### 7. Thread safety

- Single-threaded execution
- No shared mutable state across threads
- No locks, no async, no parallelism

### 8. Dependency injection

- All services registered as **singletons**
- Same instance used throughout run
- No scoped or transient lifetimes

### 9. Repository behaviour

- `JsonRepository<T>.GetAll()` returns all entities from file
- `Get(id)` returns entity or throws if not found
- File read on each call (no caching in NuciDAL)

### 10. Word/sentence identity

- `Word.Id` and `Sentence.Id` are **strings** (not numeric)
- IDs used for cross-referencing (sentences reference word IDs)
- No auto-generation — IDs provided in input files

## Data invariants

### Dictionary

- `PolyglotClass.Id` unique across parts of speech
- `PolyglotWord.WordTypeId` references existing `PolyglotClass.Id`
- `DeclesionNode.DeclensionRelatedId` references `PolyglotClass.Id`
- `DimensionNode.DimensionId` unique within declension node
- `DeclesionGenerationRule.DecGenRuleTypeId` references `PolyglotClass.Id`
- `DeclesionGenerationRule.DecGenRuleComb` format: `",id1,id2,...,idN,"`

### Words

- `Word.Id` unique across words
- `Word.PartOfSpeechName` matches existing `PolyglotClass.Name`
- `Word.Dimensions` keys match `DeclesionNode.DeclensionText` for that part of speech
- `Word.Dimensions` values match `DimensionNode.DimensionName` for corresponding node

### Sentences

- `Sentence.Id` unique across sentences
- `Sentence.Words` space-separated `Word.Id` values (all must exist)
- `Sentence.ConstructedLanguageSentence` is the expected validation target

## Behavioural invariants

### DeclesionBuilder.GetWord

1. Always returns a string (never null)
2. Returns base constructed word if no rules match
3. Applies all matching rules (not just first)
4. Rule guard regex evaluated against current word state
5. Transformations mutate word progressively

### DeclesionBuilder.GetSentence

1. Splits input by spaces
2. Calls `GetWord` for each word ID
3. Joins results with single space
4. Preserves word order

### LanguageParser lookups

- `GetWord(localWord, partOfSpeechId)` → first match where `WordTypeId == partOfSpeechId` AND `LocalWord.Contains(localWord)`
- `GetPartOfSpeech(id)` → exact ID match (throws if not found)
- `GetPartOfSpeech(name)` → exact Name match (throws if not found)
- `GetDeclesionNodes(partOfSpeechId)` → all nodes where `DeclensionRelatedId == partOfSpeechId`
- `GetDimensionNode(nodeName, partOfSpeechId, dimensionValue)` → exact match on all three
- `GetDeclesionGenerationRules(ruleCombination, partOfSpeechId)` → exact match on both

## Violation consequences

| Invariant violated | Symptom |
|---|---|
| Dictionary immutability | N/A (enforced by design) |
| Rule determinism | Non-reproducible validation |
| Validation completeness | Silent failures |
| Exit code contract | CI/CD misinterpretation |
| Output contract | Parsing failures in automation |
| File handling | Stale data, resource leaks |
| Thread safety | Race conditions (not applicable) |
| DI singletons | State leakage between runs |
| Repository behaviour | Performance issues, stale reads |
| Identity invariants | Cross-reference failures |