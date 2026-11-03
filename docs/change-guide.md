# Change guide

## Contributing changes

### 1. Fork and branch

```bash
git clone <your-fork>
cd polyglot-language-validator
git checkout -b feature/your-change
```

### 2. Make changes

- Follow existing code style
- Add tests for new behaviour
- Update documentation if interfaces change

### 3. Validate

```bash
dotnet build PolyglotLanguageValidator.slnx
dotnet test PolyglotLanguageValidator.slnx
```

### 4. Commit

```bash
git add .
git commit -m "feat: brief description

- Detail 1
- Detail 2"
```

### 5. Push and PR

```bash
git push origin feature/your-change
# Open PR on GitHub
```

## Code style

- **C#**: Standard .NET conventions (PascalCase types/methods, camelCase locals/parameters)
- **No .editorconfig** — Follow existing patterns in codebase
- **Nullable**: Not enabled — don't add `?` annotations
- **Using directives**: Grouped (System, then third-party, then project)

## Test requirements

- New public methods → unit tests
- Bug fixes → regression test
- Behaviour changes → update affected tests
- Run full test suite before PR

## Documentation updates

Update when changing:
- Public interfaces (`ILanguageParser`, `IDeclesionBuilder`, `ILanguageValidator`, `IRepository<T>`)
- CLI arguments (`InputSettings`)
- File formats (dictionary XML, words JSON, sentences JSON)
- Exit codes or output format
- Architecture (new layers, changed dependencies)

Files to update:
- `docs/api-reference/INDEX.md`
- `docs/configuration.md`
- `docs/architecture.md`
- `docs/data-model.md`
- `docs/quick-start.md`
- `docs/api-usage-examples.md`

## Versioning

- Semantic versioning (MAJOR.MINOR.PATCH)
- MAJOR: Breaking changes (interface changes, file format changes)
- MINOR: New features (backward compatible)
- PATCH: Bug fixes

## Release process

1. Update version in release script or csproj
2. Run `./release.sh <version>`
3. Script handles tagging, packaging, publishing

## Breaking change checklist

- [ ] Interface method added/removed/changed
- [ ] File format schema changed
- [ ] CLI argument added/removed/changed
- [ ] Exit code meaning changed
- [ ] Output format changed
- [ ] Dependency version bumped (major)

If any checked → MAJOR version bump required.

## Review criteria

PRs reviewed for:
- Correctness (tests pass, logic sound)
- Consistency (style, patterns, naming)
- Documentation (updated if needed)
- Simplicity (no over-engineering)
- Performance (no regressions)