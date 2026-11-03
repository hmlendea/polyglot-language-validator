# Dependencies

## Production dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| Microsoft.Extensions.DependencyInjection | 10.0.5 | MIT | DI container |
| NuciCLI.Arguments | 1.0.1 | MIT | CLI argument parsing |
| NuciDAL | 3.0.1 | MIT | JSON repository, EntityBase |

## Test dependencies

| Package | Version | License | Purpose |
|---|---|---|---|
| Microsoft.NET.Test.Sdk | 17.13.0 | MIT | Test host |
| NUnit | 4.3.2 | MIT | Test framework |
| NUnit3TestAdapter | 5.0.0 | MIT | VS Code/CLI adapter |
| Moq | 4.20.72 | BSD-3-Clause | Mocking |

## Transitive dependencies (key)

### Microsoft.Extensions.DependencyInjection →
- Microsoft.Extensions.DependencyInjection.Abstractions
- System.ComponentModel

### NuciCLI.Arguments →
- System.CommandLine (or similar parsing library)

### NuciDAL →
- Newtonsoft.Json or System.Text.Json
- File I/O abstractions

### Moq →
- Castle.Core (dynamic proxy)

## Dependency graph

```
PolyglotLangageValidator
├── Microsoft.Extensions.DependencyInjection
├── NuciCLI.Arguments
└── NuciDAL
    └── (JSON serializer)

PolyglotLanguageValidator.UnitTests
├── Microsoft.NET.Test.Sdk
├── NUnit
├── NUnit3TestAdapter
├── Moq
└── PolyglotLangageValidator (project reference)
```

## Version policy

- **Pinned versions** — All packages use exact versions (no floating)
- **No central package management** — Versions in each csproj
- **No Directory.Packages.props** — Not used

## Security

- All packages from nuget.org
- No private feeds
- Regular `dotnet list package --vulnerable --include-transitive` recommended

## Updates

```bash
# Check outdated
dotnet list package --outdated

# Update specific
dotnet add PolyglotLangageValidator/PolyglotLangageValidator.csproj package NuciDAL --version 3.0.2

# Update all (interactive)
dotnet outdated
```

## License compliance

All dependencies use permissive licenses (MIT, BSD-3-Clause). No copyleft licenses. Compatible with commercial use.