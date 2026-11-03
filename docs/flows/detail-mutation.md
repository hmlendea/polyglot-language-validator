# Flows — Detail mutation

## Word mutation flow (declension application)

```
DeclesionBuilder.GetWord(Word)
    │
    ▼
1. Resolve part of speech
    languageParser.GetPartOfSpeech(word.PartOfSpeechName)
    → PolyglotClass{Id, Name}
    │
    ▼
2. Get base constructed word
    languageParser.GetWord(word.LocalWord, partOfSpeech.Id)
    → Searches lexicon for WordTypeId match AND LocalWord contains input
    → Returns PolyglotWord.ConstructedWord (e.g., "fel")
    │
    ▼
3. Build rule combination from dimensions
    ruleCombination = ","
    For each declensionNode in languageParser.GetDeclesionNodes(partOfSpeech.Id):
        dimensionValue = word.Dimensions[declensionNode.Name]
        dimension = languageParser.GetDimensionNode(declensionNode.Name, partOfSpeech.Id, dimensionValue)
        ruleCombination += $"{dimension.Id},"
    │
    ▼
4. Find matching generation rules
    languageParser.GetDeclesionGenerationRules(ruleCombination, partOfSpeech.Id)
    → Filters by RuleCombination AND PartOfSpeechId
    → Returns ordered list
    │
    ▼
5. Apply rules sequentially
    currentWord = baseConstructedWord
    For each rule in matchingRules:
        If Regex.Matches(currentWord, rule.RegularExpression).Any():
            For each transformation in rule.Transformations:
                currentWord = Regex.Replace(currentWord, transformation.Pattern, transformation.Replacement)
    │
    ▼
6. Return mutated word
```

## Rule matching details

### RuleCombination structure

```
Format: ",<dim1Id>,<dim2Id>,...,<dimNId>,"
- Leading and trailing commas
- Dimension IDs in declension node order (XML order)
- One ID per declension node for the part of speech
```

### RegularExpression purpose

- Acts as a **guard condition** — rule only applies if regex matches current word state
- Evaluated against the word **after previous rules/transformations**
- Common patterns:
  - `$` — matches end of string (always matches)
  - `^` — matches start of string
  - Specific patterns — e.g., `s$` matches words ending in 's'

### Transformation application

```
For each transformation in rule.Transformations (in order):
    currentWord = Regex.Replace(currentWord, transformation.Pattern, transformation.Replacement)
```

- `Pattern` — Regex pattern (can use capture groups)
- `Replacement` — Replacement string (can use $1, $2, etc. for capture groups)
- Applied to the **current state** of the word (mutates progressively)

## Example: Noun pluralisation

### Dictionary setup

```xml
<declensionNode>
    <declensionId>1</declensionId>
    <declensionText>number</declensionText>
    <declensionRelatedId>1</declensionRelatedId>
    <dimensionNode><dimensionId>1</dimensionId><dimensionName>singular</dimensionName></dimensionNode>
    <dimensionNode><dimensionId>2</dimensionId><dimensionName>plural</dimensionName></dimensionNode>
</declensionNode>

<decGenRule>
    <decGenRuleComb>,1,</decGenRuleComb>
    <decGenRuleRegex>$</decGenRuleRegex>
    <decGenRuleName>singular</decGenRuleName>
    <decGenRuleTypeId>1</decGenRuleTypeId>
    <decGenRuleIndex>1</decGenRuleIndex>
</decGenRule>

<decGenRule>
    <decGenRuleComb>,2,</decGenRuleComb>
    <decGenRuleRegex>$</decGenRuleRegex>
    <decGenRuleName>plural</decGenRuleName>
    <decGenRuleTypeId>1</decGenRuleTypeId>
    <decGenRuleIndex>1</decGenRuleIndex>
    <decGenTrans>
        <decGenTransRegex>$</decGenTransRegex>
        <decGenTransReplace>i</decGenTransReplace>
    </decGenTrans>
</decGenRule>
```

### Execution for singular

```
Word{LocalWord="cat", PartOfSpeechName="noun", Dimensions={"number":"singular"}}

1. partOfSpeech = GetPartOfSpeech("noun") → Id=1
2. baseWord = GetWord("cat", 1) → "fel"
3. nodes = GetDeclesionNodes(1) → [Node{Id=1, Name="number"}]
4. ruleCombination:
   - node "number": dimension "singular" → Id=1
   - ruleCombination = ",1,"
5. rules = GetDeclesionGenerationRules(",1,", 1) → [singularRule]
6. Apply singularRule:
   - Regex.Matches("fel", "$") → match
   - Transformations: [Pattern="$", Replacement=""] (empty in example)
   - Result: "fel"
7. Return "fel"
```

### Execution for plural

```
Word{LocalWord="cat", PartOfSpeechName="noun", Dimensions={"number":"plural"}}

1. partOfSpeech = GetPartOfSpeech("noun") → Id=1
2. baseWord = GetWord("cat", 1) → "fel"
3. nodes = GetDeclesionNodes(1) → [Node{Id=1, Name="number"}]
4. ruleCombination:
   - node "number": dimension "plural" → Id=2
   - ruleCombination = ",2,"
5. rules = GetDeclesionGenerationRules(",2,", 1) → [pluralRule]
6. Apply pluralRule:
   - Regex.Matches("fel", "$") → match
   - Transformations: [Pattern="$", Replacement="i"]
   - Regex.Replace("fel", "$", "i") → "feli"
7. Return "feli"
```

## Example: Verb tense

### Dictionary setup

```xml
<declensionNode>
    <declensionId>2</declensionId>
    <declensionText>tense</declensionText>
    <declensionRelatedId>2</declensionRelatedId>
    <dimensionNode><dimensionId>3</dimensionId><dimensionName>present</dimensionName></dimensionNode>
</declensionNode>

<decGenRule>
    <decGenRuleComb>,3,</decGenRuleComb>
    <decGenRuleRegex>$</decGenRuleRegex>
    <decGenRuleName>present</decGenRuleName>
    <decGenRuleTypeId>2</decGenRuleTypeId>
    <decGenRuleIndex>1</decGenRuleIndex>
    <decGenTrans>
        <decGenTransRegex>$</decGenTransRegex>
        <decGenTransReplace>t</decGenTransReplace>
    </decGenTrans>
</decGenRule>
```

### Execution

```
Word{LocalWord="run", PartOfSpeechName="verb", Dimensions{"tense":"present"}}

1. partOfSpeech = GetPartOfSpeech("verb") → Id=2
2. baseWord = GetWord("run", 2) → "cur"
3. nodes = GetDeclesionNodes(2) → [Node{Id=2, Name="tense"}]
4. ruleCombination:
   - node "tense": dimension "present" → Id=3
   - ruleCombination = ",3,"
5. rules = GetDeclesionGenerationRules(",3,", 2) → [presentRule]
6. Apply presentRule:
   - Regex.Matches("cur", "$") → match
   - Regex.Replace("cur", "$", "t") → "curt"
7. Return "curt"
```

## Multi-dimension example

### Dictionary with two nodes for nouns

```xml
<declensionNode>
    <declensionId>1</declensionId>
    <declensionText>number</declensionText>
    <declensionRelatedId>1</declensionRelatedId>
    <dimensionNode><dimensionId>1</dimensionId><dimensionName>singular</dimensionName></dimensionNode>
    <dimensionNode><dimensionId>2</dimensionId><dimensionName>plural</dimensionName></dimensionNode>
</declensionNode>

<declensionNode>
    <declensionId>4</declensionId>
    <declensionText>case</declensionText>
    <declensionRelatedId>1</declensionRelatedId>
    <dimensionNode><dimensionId>5</dimensionId><dimensionName>nominative</dimensionName></dimensionNode>
    <dimensionNode><dimensionId>6</dimensionId><dimensionName>accusative</dimensionName></dimensionNode>
</declensionNode>

<decGenRule>
    <decGenRuleComb>,2,6,</decGenRuleComb>
    <decGenRuleRegex>$</decGenRuleRegex>
    <decGenRuleName>plural-accusative</decGenRuleName>
    <decGenRuleTypeId>1</decGenRuleTypeId>
    <decGenRuleIndex>1</decGenRuleIndex>
    <decGenTrans>
        <decGenTransRegex>$</decGenTransRegex>
        <decGenTransReplace>m</decGenTransReplace>
    </decGenTrans>
</decGenRule>
```

### Execution for plural accusative

```
Word{LocalWord="cat", PartOfSpeechName="noun", Dimensions{"number":"plural", "case":"accusative"}}

1. partOfSpeech = GetPartOfSpeech("noun") → Id=1
2. baseWord = GetWord("cat", 1) → "fel"
3. nodes = GetDeclesionNodes(1) → [Node{Id=1,Name="number"}, Node{Id=4,Name="case"}]
4. ruleCombination:
   - node "number": dimension "plural" → Id=2
   - node "case": dimension "accusative" → Id=6
   - ruleCombination = ",2,6,"
5. rules = GetDeclesionGenerationRules(",2,6,", 1) → [pluralAccusativeRule]
6. Apply rule:
   - Regex.Matches("fel", "$") → match
   - Regex.Replace("fel", "$", "m") → "felm"
7. Return "felm"
```

## Rule ordering and precedence

1. **XML order** — Rules appear in the order they are defined in the XML
2. **All matching rules apply** — Not just the first match; all rules with matching RuleCombination and PartOfSpeechId are evaluated
3. **Guard regex** — Each rule's RegularExpression must match the current word state
4. **Transformation order** — Within a rule, transformations apply in XML order
5. **Cumulative effect** — Each transformation mutates the word for subsequent rules/transformations

## Edge cases

### No matching rules

```
ruleCombination = ",999," (non-existent dimension)
rules = GetDeclesionGenerationRules(",999,", 1) → []
Result: base word returned unchanged
```

### Multiple matching rules

```
Rules for ",1,": [ruleA, ruleB]
ruleA: Regex="^f", Transform: "f"→"ph"
ruleB: Regex="$", Transform: "$"→"s"

Word: "fel"
1. ruleA: Matches "^f" → "phel"
2. ruleB: Matches "$" on "phel" → "phels"
Result: "phels"
```

### Transformation with capture groups

```
Transformation: Pattern="(.+)s$", Replacement="$1z"
Word: "cats"
Regex.Replace("cats", "(.+)s$", "$1z") → "catz"
```

### Empty transformation list

```
Rule with no <decGenTrans> elements
→ Transformations = empty list
→ Rule matches but no changes applied
→ Word passes through unchanged (but subsequent rules still evaluate)
```