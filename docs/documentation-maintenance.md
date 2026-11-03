# Documentation maintenance

## Principles

- **Documentation as code** — Stored in `docs/`, versioned with source
- **Single source of truth** — Code is authoritative; docs reflect code
- **Update with changes** — PRs must update affected docs
- **No duplication** — Link instead of copying

## Structure

```
docs/
├── INDEX.md                          # Master navigation
├── repository-overview.md            # High-level description
├── repository-structure.md           # File tree
├── architecture.md                   # System architecture
├── data-model.md                     # Entities and relationships
├── build-and-deployment.md           # Build, test, release
├── configuration.md                  # CLI args, file formats
├── quick-start.md                    # 5-minute start
├── testing.md                        # Test patterns
├── dependencies.md                   # Package list
├── design-decisions.md               # Architectural choices
├── error-handling.md                 # Exceptions, exit codes
├── logging.md                        # Logging (minimal)
├── security.md                       # Threat model
├── troubleshooting.md                # Common issues
├── faq.md                            # Frequently asked
├── invariants.md                     # System guarantees
├── concurrency-and-scheduling.md     # Threading model
├── state-and-persistence.md          # Data flow
├── integrations.md                   # External deps
├── api-usage-examples.md             # CLI examples
├── ambiguities-and-open-questions.md # Known gaps
├── change-guide.md                   # Contribution guide
└── documentation-maintenance.md      # This file
```

## Update triggers

| Change type | Docs to update |
|---|---|
| New CLI argument | configuration.md, quick-start.md, api-usage-examples.md |
| Interface change | api-reference/INDEX.md, architecture.md, design-decisions.md |
| File format change | configuration.md, data-model.md, quick-start.md |
| New dependency | dependencies.md, integrations.md, build-and-deployment.md |
| Behaviour change | flows/, components/, invariants.md, troubleshooting.md |
| Test pattern change | testing.md |
| Release process change | build-and-deployment.md, change-guide.md |

## Review checklist

Before merging PR with code changes:

- [ ] Does any public interface change? → Update `api-reference/INDEX.md`
- [ ] Does CLI change? → Update `configuration.md`, `quick-start.md`
- [ ] Does file format change? → Update `configuration.md`, `data-model.md`
- [ ] Does architecture change? → Update `architecture.md`, `design-decisions.md`
- [ ] New error type? → Update `error-handling.md`, `troubleshooting.md`
- [ ] New dependency? → Update `dependencies.md`, `integrations.md`
- [ ] Behaviour change? → Update relevant `flows/`, `components/`, `invariants.md`

## Style guide

- **Headings**: Sentence case (`# Build and deployment`)
- **Code symbols**: Backticks (`LanguageValidator`, `GetWord`)
- **File paths**: Relative, backticks (`docs/architecture.md`)
- **Links**: Relative, verified (`[Architecture](architecture.md)`)
- **Tables**: For structured data (dependencies, arguments, exit codes)
- **Code blocks**: Language tag (`bash`, `csharp`, `xml`, `json`)
- **No duplication**: Link to canonical location

## Link verification

```bash
# Check all internal links
grep -r '\[.*\](.*\.md)' docs/ | grep -v 'http' | while read line; do
  file=$(echo "$line" | cut -d: -f1)
  link=$(echo "$line" | sed 's/.*(\(.*\)).*/\1/')
  target="docs/$link"
  [ -f "$target" ] || echo "BROKEN: $file -> $link"
done
```

## Adding new documents

1. Create in `docs/` with kebab-case name
2. Add entry to `INDEX.md` catalogue
3. Follow structure: Overview → Details → Examples → References
4. Cross-link related documents

## Deprecating documents

1. Add notice at top: `> **Deprecated** — See [New Document](new-doc.md)`
2. Update `INDEX.md` to link to replacement
3. Keep file for link integrity (don't delete)

## Tooling

- **Editor**: VS Code with Markdown preview
- **Linting**: `markdownlint` (optional)
- **Spell check**: `cspell` (optional)
- **Link check**: Manual or CI step

## Ownership

- **Maintainers**: Review doc changes in PRs
- **Contributors**: Update docs with code changes
- **No dedicated tech writer** — Developer-maintained