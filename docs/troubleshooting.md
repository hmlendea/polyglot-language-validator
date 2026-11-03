# Troubleshooting

## Common issues

### "Could not find file" / FileNotFoundException

**Cause**: Input file path incorrect or file missing.

**Fix**:
```bash
# Verify paths exist
ls -la dictionary.xml words.json sentences.json

# Use absolute paths
dotnet run -- --dictionary /full/path/dictionary.xml --words /full/path/words.json --sentences /full/path/sentences.json
```

### "The actual value 'X' does not match the expected value 'Y'"

**Cause**: Generated sentence doesn't match expected in sentences.json.

**Debug**:
1. Check dictionary has correct base word for the local word
2. Verify declension rules exist for the dimension combination
3. Ensure rule transformations produce expected output
4. Run with test dictionary to isolate rule logic

### Exit code 1 but no output

**Cause**: AggregateException caught but inner exceptions not printed.

**Fix**: Ensure `Program.cs` catches `AggregateException` and iterates `ex.InnerExceptions`.

### "Sequence contains no matching element" (InvalidOperationException)

**Cause**: `LanguageParser.GetWord` or `GetPartOfSpeech` uses `First()` on empty sequence.

**Fix**: Verify dictionary contains the expected `PolyglotClass` and `PolyglotWord` entries.

### "Object reference not set to an instance of an object" (NullReferenceException)

**Cause**: Missing null check in `LanguageParser` or `DeclesionBuilder`.

**Common locations**:
- `GetDimensionNode` returns null → ruleCombination malformed
- `GetDeclesionGenerationRules` returns empty → no rules applied
- Word `Dimensions` dictionary missing key for declension node

### Dictionary XML not loading

**Cause**: XML structure doesn't match `PolyglotDictionary` model.

**Check**:
- Root element `<PolyglotDictionary>`
- `<PartsOfSpeech>` with `<PolyglotClass Id="..." Name="..." />`
- `<Lexicon>` with `<PolyglotWord WordTypeId="..." LocalWord="..." ConstructedWord="..." />`
- `<DeclesionCollection>` with `<DeclesionNodes>` and `<DeclesionGenerationRules>`

### Words JSON not loading

**Cause**: JSON doesn't match `Word` model.

**Required fields**:
```json
{
  "Id": "string",
  "LocalWord": "string",
  "PartOfSpeechName": "string",
  "Dimensions": { "key": "value" }
}
```

### Sentences JSON not loading

**Cause**: JSON doesn't match `Sentence` model.

**Required fields**:
```json
{
  "Id": "string",
  "Words": "space-separated-ids",
  "ConstructedLanguageSentence": "string",
  "LocalLanguageSentence": "string"
}
```

### Rule not applying

**Cause**: RuleCombination doesn't match any generation rule.

**Debug**:
1. Print ruleCombination in `DeclesionBuilder.GetWord`
2. Verify dimension IDs match dictionary
3. Check `DecGenRuleComb` in XML matches exactly (including leading/trailing commas)

### Multiple rules applying unexpectedly

**Cause**: Multiple rules share same RuleCombination and PartOfSpeechId.

**Behaviour**: All matching rules apply sequentially (by XML order).

**Fix**: Ensure RuleCombination uniquely identifies rule set, or use `DecGenRuleIndex` for ordering.

### Build fails: "The type or namespace name 'NuciDAL' could not be found"

**Cause**: Packages not restored.

**Fix**:
```bash
dotnet restore PolyglotLanguageValidator.slnx
```

### Tests fail: "Could not load file or assembly 'Moq'"

**Cause**: Test packages not restored.

**Fix**:
```bash
dotnet restore PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj
```

## Debugging tips

### Enable verbose output

```bash
# Add temporary Console.WriteLine in DeclesionBuilder.GetWord
Console.WriteLine($"RuleCombination: {ruleCombination}");
Console.WriteLine($"Matching rules: {rules.Count()}");
foreach (var r in rules) Console.WriteLine($"  Rule: {r.DecGenRuleName}, Regex: {r.DecGenRuleRegex}");
```

### Inspect loaded dictionary

```csharp
// In LanguageParser constructor after deserialization
Console.WriteLine($"Parts of speech: {dictionary.PartsOfSpeech.Count}");
Console.WriteLine($"Lexicon entries: {dictionary.Lexicon.Count}");
Console.WriteLine($"Declension nodes: {dictionary.DeclesionCollection.DeclesionNodes.Count}");
Console.WriteLine($"Generation rules: {dictionary.DeclesionCollection.DeclesionGenerationRules.Count}");
```

### Test single word

```bash
# Create minimal test files
echo '[{"Id":"t1","LocalWord":"test","PartOfSpeechName":"noun","Dimensions":{"number":"singular"}}]' > test-words.json
echo '[{"Id":"t1","Words":"t1","ConstructedLanguageSentence":"expected","LocalLanguageSentence":"test"}]' > test-sentences.json
dotnet run -- --dictionary dict.xml --words test-words.json --sentences test-sentences.json
```

## Getting help

1. Check [FAQ](faq.md)
2. Review [error-handling.md](error-handling.md) for exception details
3. Examine unit tests for expected behaviour
4. Open issue with: input files, command run, full output, .NET version