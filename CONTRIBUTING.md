# Contributing to Polyglot Language Validator

This document covers the guidelines and processes for contributing to this project, including how to report issues, suggest enhancements, submit code changes, and follow project standards.

## 📑 Table of Contents

- [How to Contribute](#how-to-contribute)
- [Code of Conduct](#code-of-conduct)
- [Security](#security)
- [License](#license)

## 🤝 How to Contribute

### Reporting Issues

- Search existing issues first.
- Use the issue templates if available.
- Provide clear reproduction steps.
- Include environment details.

### Suggesting Enhancements

- Check the roadmap and existing discussions.
- Explain the use case and expected behaviour.
- Consider implementation complexity.

### Code Contributions

#### Prerequisites

- .NET 10.0 SDK or later

#### Development Setup

```bash
# Clone the repository
git clone https://github.com/hmlendea/polyglot-language-validator.git
cd polyglot-language-validator

# Restore dependencies
dotnet restore
```

#### Making Changes

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Make your changes.
4. Run tests: `dotnet test PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj`
5. Run build: `dotnet build PolyglotLanguageValidator.csproj`
6. Commit with clear and descriptive messages.
7. Push to your fork.
8. Open a Pull Request.

#### Pull Request Guidelines

- Target the `master` branch.
- Keep PRs focused and atomic.
- Revise documentation if applicable.
- Include tests for any new functionality.
- Ensure the CI checks pass.

### Code Style

Follow the project's coding standards:
- C# coding conventions as defined in the repository instructions
- Run `dotnet format` before committing (if available)

### Testing

```bash
# Run all tests
dotnet test PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj
```

### Documentation

- Revise relevant docs for changes.
- Follow the documentation style guide.
- Preview changes locally if possible.

## 📋 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🔒 Security

See the [Security Policy](./SECURITY.md) for information on reporting security vulnerabilities.

## 📄 License

Licensed under the GNU General Public License v3.0 or later.
See [LICENSE](./LICENSE) for details.