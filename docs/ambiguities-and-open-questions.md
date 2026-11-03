# Ambiguities and open questions

## Known ambiguities

### 1. Word lookup matching logic

**Current**: `LanguageParser.GetWord(localWord, partOfSpeechId)` uses `LocalWord.Contains(localWord)`.

**Ambiguity**:
- Partial match — "cat" matches "cat", "cats", "scatter"
- Case sensitivity — uses .NET default (case-sensitive)
- Multiple matches — returns `First()` (XML order)

**Question**: Should this be exact match? Case-insensitive? Longest match?

### 2. RuleCombination uniqueness

**Current**: Multiple rules can have identical `DecGenRuleComb` + `DecGenRuleTypeId`.

**Ambiguity**: All matching rules apply sequentially. Is this intentional for cumulative transformations, or should only one rule match?

**Question**: Should RuleCombination be unique per part of speech?

### 3. Dimension key matching

**Current**: `Word.Dimensions` keys must match `DeclesionNode.DeclensionText` exactly.

**Ambiguity**:
- Case sensitivity
- Whitespace handling
- Synonyms/aliases not supported

**Question**: Should keys be normalized (lowercase, trimmed)?

### 4. Sentence word order

**Current**: `Sentence.Words` is space-separated IDs; `GetSentence` joins with single space.

**Ambiguity**:
- Multiple spaces collapsed
- No punctuation handling
- No word boundary markers

**Question**: Should sentence structure be more explicit (array of IDs, punctuation tokens)?

### 5. Empty transformation list

**Current**: Rule with no `<DeclesionGenerationRuleTransformation>` elements matches but applies no changes.

**Ambiguity**: Is this a "blocking rule" (prevents other rules) or a no-op?

**Question**: Document intended semantics or disallow empty transformations.

### 6. RegularExpression guard behaviour

**Current**: Rule's `DecGenRuleRegex` evaluated against current word state after previous rules.

**Ambiguity**:
- Regex options (case sensitivity, multiline, etc.)
- Anchoring (^/$) implicit or explicit?
- Performance with complex regexes

**Question**: Should regex options be configurable or fixed?

## Open questions

### 1. Extensibility vs simplicity

**Tension**: Interface-based DI enables extension, but tool is simple CLI.

**Question**: Is the abstraction overhead justified? Should we simplify to concrete classes?

### 2. Dictionary format lock-in

**Current**: Tied to Polyglot XML schema via `LanguageParser`.

**Question**: Should we define a neutral intermediate representation to support multiple dictionary formats?

### 3. Validation granularity

**Current**: All-or-nothing — entire sentence must match exactly.

**Question**: Should we support partial validation, word-level reporting, or similarity scoring?

### 4. Performance for large dictionaries

**Current**: Full XML deserialization at startup; LINQ `First()`/`FirstOrDefault()` on lists.

**Question**: At what scale does this become a bottleneck? Should we add indexing?

### 5. Error reporting richness

**Current**: Single message format for all mismatches.

**Question**: Should we add structured output (JSON), error codes, or diff-style output?

### 6. Configuration evolution

**Current**: CLI args only, no config file.

**Question**: Will we need config files for complex projects? If so, what format?

### 7. Test data management

**Current**: Inline test data in unit tests.

**Question**: Should we extract shared test fixtures for dictionary/words/sentences?

### 8. Release automation

**Current**: Manual `release.sh` with external script.

**Question**: Should we migrate to GitHub Actions release workflow for better auditability?

### 9. .NET version strategy

**Current**: Targets net10.0 (preview/current).

**Question**: LTS vs current? Multi-targeting? Support policy?

### 10. NuciDAL/NuciCLI dependency risk

**Current**: Depends on hmlendea packages (NuciDAL, NuciCLI.Arguments).

**Question**:
- Are these maintained long-term?
- Should we vendor/copy critical code?
- What if packages become unavailable?

## Decision tracking

| Item | Status | Decision | Date |
|---|---|---|---|
| Word lookup exact match | Open | — | — |
| RuleCombination uniqueness | Open | — | — |
| Dimension key normalization | Open | — | — |
| Sentence structure | Open | — | — |
| Empty transformation semantics | Open | — | — |
| Regex options | Open | — | — |
| Abstraction level | Open | — | — |
| Dictionary format neutrality | Open | — | — |
| Validation granularity | Open | — | — |
| Performance indexing | Open | — | — |
| Structured error output | Open | — | — |
| Config file support | Open | — | — |
| Test fixtures | Open | — | — |
| Release automation | Open | — | — |
| .NET version policy | Open | — | — |
| Nuci package risk | Open | — | — |

## Resolution process

1. Document ambiguity in this file
2. Discuss in issue/PR
3. Decide and record in table above
4. Update code and docs accordingly
5. Close item