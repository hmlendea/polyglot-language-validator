# Documentation index

## Repository summary

**Polyglot Language Validator** — A .NET 10.0 command-line tool that validates constructed language outputs against expected results using Polyglot dictionary XML, words JSON, and sentences JSON.

- **Type**: Console application (CLI)
- **Framework**: .NET 10.0
- **Architecture**: Clean layered with dependency injection
- **Input**: Dictionary (XML), Words (JSON), Sentences (JSON)
- **Output**: Validation results (exit code + stdout)
- **Testing**: NUnit 4.x + Moq

## Root document map

| Document | Purpose |
|---|---|
| [repository-overview.md](repository-overview.md) | High-level project description, goals, scope |
| [repository-structure.md](repository-structure.md) | File/folder tree with descriptions |
| [architecture.md](architecture.md) | System architecture, layers, dependencies |
| [data-model.md](data-model.md) | Core entities, relationships, serialization |
| [build-and-deployment.md](build-and-deployment.md) | Build, test, release, publish, CI/CD |
| [configuration.md](configuration.md) | CLI arguments, environment, settings |
| [quick-start.md](quick-start.md) | Getting started in 5 minutes |
| [testing.md](testing.md) | Test structure, patterns, coverage |
| [dependencies.md](dependencies.md) | Production and test dependencies |
| [design-decisions.md](design-decisions.md) | Key architectural choices and rationale |
| [error-handling.md](error-handling.md) | Exception types, handling strategies |
| [logging.md](logging.md) | Logging infrastructure and practices |
| [security.md](security.md) | Security considerations |
| [troubleshooting.md](troubleshooting.md) | Common issues and solutions |
| [faq.md](faq.md) | Frequently asked questions |
| [invariants.md](invariants.md) | System invariants and guarantees |
| [concurrency-and-scheduling.md](concurrency-and-scheduling.md) | Threading model (single-threaded) |
| [state-and-persistence.md](state-and-persistence.md) | File-based state, no database |
| [integrations.md](integrations.md) | External dependencies (NuGet packages) |
| [ambiguities-and-open-questions.md](ambiguities-and-open-questions.md) | Known gaps and future work |
| [documentation-maintenance.md](documentation-maintenance.md) | How to maintain these docs |
| [change-guide.md](change-guide.md) | How to contribute changes |
| [api-usage-examples.md](api-usage-examples.md) | CLI usage examples |

## Documentation catalogue

### Flows

| Document | Description |
|---|---|
| [flows/startup-and-rendering.md](flows/startup-and-rendering.md) | Application startup, DI setup, CLI parsing, validation orchestration |
| [flows/calendar-search.md](flows/calendar-search.md) | Dictionary loading, word/sentence lookup, rule resolution |
| [flows/detail-mutation.md](flows/detail-mutation.md) | Word mutation via declension rules, transformations, examples |

### Components

| Document | Description |
|---|---|
| [components/presentation.md](components/presentation.md) | CLI layer: Program.cs, InputSettings.cs |
| [components/application-services.md](components/application-services.md) | Service layer: LanguageValidator, DeclesionBuilder, LanguageParser |

### Behaviour

| Document | Description |
|---|---|
| [behaviour/browse-and-search.md](behaviour/browse-and-search.md) | Dictionary traversal, word/sentence lookup patterns |
| [behaviour/inspect-edit-delete.md](behaviour/inspect-edit-delete.md) | Not applicable (CLI tool, no CRUD UI) |

### API reference

| Document | Description |
|---|---|
| [api-reference/INDEX.md](api-reference/INDEX.md) | CLI interface, internal service interfaces, extension points |

## Navigation aids

### By role

- **New contributor**: Start with [quick-start.md](quick-start.md) → [repository-overview.md](repository-overview.md) → [architecture.md](architecture.md)
- **Developer**: [architecture.md](architecture.md) → [components/](components/) → [flows/](flows/)
- **Maintainer**: [build-and-deployment.md](build-and-deployment.md) → [testing.md](testing.md) → [release.sh](../release.sh)
- **User**: [quick-start.md](quick-start.md) → [configuration.md](configuration.md) → [api-usage-examples.md](api-usage-examples.md)

### By task

- **Run the validator**: [quick-start.md](quick-start.md)
- **Understand rule engine**: [flows/detail-mutation.md](flows/detail-mutation.md)
- **Add test**: [testing.md](testing.md)
- **Release**: [build-and-deployment.md](build-and-deployment.md)
- **Extend parser**: [components/application-services.md](components/application-services.md) → [api-reference/INDEX.md](api-reference/INDEX.md)

## Maintenance metadata

- **Generated**: 2025 (during documentation initiative)
- **Source of truth**: Source code in `PolyglotLangageValidator/` and `PolyglotLanguageValidator.UnitTests/`
- **Update trigger**: Any architectural, behavioural, or interface change
- **Review cadence**: Per PR affecting documented areas
- **Owner**: Repository maintainers

## Link integrity

All links in this index are relative and verified against the `docs/` directory structure. Run `find docs -name "*.md" | sort` to audit completeness.