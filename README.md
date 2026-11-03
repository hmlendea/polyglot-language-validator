[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/polyglot-language-validator)](https://github.com/hmlendea/polyglot-language-validator/releases/latest)
[![Build Status](https://github.com/hmlendea/polyglot-language-validator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/polyglot-language-validator/actions/workflows/dotnet.yml)
[![License](https://img.shields.io/github/license/hmlendea/polyglot-language-validator)](https://github.com/hmlendea/polyglot-language-validator/blob/master/LICENSE)

# Polyglot Language Validator

A .NET 10.0 command-line tool that validates constructed language outputs against expected results using Polyglot dictionary XML, words JSON, and sentences JSON.

## 📑 Table of Contents

- [Capabilities](#capabilities)
- [Use Cases](#use-cases)
- [Usage](#usage)
  - [Examples](#examples)
- [Command Reference](#command-reference)
- [Compatibility](#compatibility)
- [Installation](#installation)
  - [Manual Installation](#manual-installation)
- [Configuration](#configuration)
  - [Settings](#settings)
- [Privacy and Data](#privacy-and-data)
- [Development](#development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Run](#run)
  - [Test](#test)
  - [Release](#release)
  - [Dependencies](#dependencies)
- [GitHub Actions](#github-actions)
- [Project Structure](#project-structure)
  - [Projects and Packages](#projects-and-packages)
  - [Directories](#directories)
- [Architecture](#architecture)
- [Documentation](#documentation)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [Code of Conduct](#code-of-conduct)
- [Related Projects](#related-projects)
- [Security](#security)
- [Project Engagement](#project-engagement)
- [License](#license)

## ✨ Capabilities

- Validates constructed language sentences against expected outputs.
- Processes Polyglot dictionary XML with parts of speech, lexicon, and declension rules.
- Applies regex-based declension generation rules to transform words.
- Compares generated sentences with expected outputs.
- Exits with non-zero status on validation failures for CI/CD integration.

## 🎯 Use Cases

- **Language development:** Verify that constructed language morphology produces the expected words and sentences.
- **Automated validation:** Run language-output checks in local development or CI pipelines.

## 🚀 Usage

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  --dictionary ./data/dictionary.xml \
  --words ./data/words.json \
  --sentences ./data/sentences.json
```

The command exits with code 0 when validation succeeds. Mismatches are printed and the command exits with code 1 when validation fails.

### Examples

See [docs/quick-start.md](docs/quick-start.md) for complete dictionary XML, words JSON, and sentences JSON examples.

## ⌨️ Command Reference

| Command | Description |
|---------|-------------|
| `--dictionary`, `-d` | Path to the Polyglot dictionary XML (required) |
| `--words`, `-w` | Path to words JSON (required) |
| `--sentences`, `-s` | Path to sentences JSON (optional; required for sentence validation) |

## 🧩 Compatibility

| Component | Supported Versions | Notes |
|-----------|--------------------|-------|
| .NET SDK/runtime | 10.0 | Required by the project and CI workflow. |
| Operating system | Linux, macOS, Windows | Published GitHub Release archives are provided for these platforms. |

## 📦 Installation

[![Obtain it from GitHub](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/github.png)](https://github.com/hmlendea/polyglot-language-validator/releases)

### Manual Installation

Download the archive for your platform from the [latest GitHub Release](https://github.com/hmlendea/polyglot-language-validator/releases/latest), extract it, and run the included executable with the required input-file arguments.

## ⚙️ Configuration

The application accepts input paths as command-line arguments. Dictionary and words files are required; the sentences file is optional.

### Settings

The subsequent settings are recognised:
| Section | Key | Type | Default | Required | Description |
|---------|-----|------|---------|----------|-------------|
| Input | `--dictionary` | Path | — | Yes | Polyglot dictionary XML file. |
| Input | `--words` | Path | — | Yes | Words JSON file. |
| Input | `--sentences` | Path | `null` | No | Sentences JSON file used for validation. |

## 🛡️ Privacy and Data

Polyglot Language Validator processes user-provided dictionary XML, words JSON, and sentences JSON locally, and reports validation results to the console. It performs no network communication, telemetry, or external data transfers.

For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

```bash
git clone https://github.com/hmlendea/polyglot-language-validator.git
cd polyglot-language-validator
dotnet restore PolyglotLanguageValidator.slnx
```

### Build

```bash
dotnet build PolyglotLanguageValidator.slnx
```

### Run

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  --dictionary dictionary.xml --words words.json --sentences sentences.json
```

### Test

```bash
dotnet test PolyglotLanguageValidator.slnx
```

### Release

The repository includes `release.sh`, which delegates to the upstream deployment script used by the project maintainer.

```bash
bash ./release.sh 1.0.2
```

This script downloads and executes an external release helper from `https://raw.githubusercontent.com/hmlendea/deployment-scripts/master/release/dotnet/10.0.sh`.

**Note:** Piping into `bash` is an intensely controversial topic. Please review any external scripts before running them in your environment!

### Dependencies

| Package | Version | Scope | Purpose |
|---------|---------|-------|---------|
| Microsoft.Extensions.DependencyInjection | 10.0.14 | Runtime | Dependency injection container. |
| NuciCLI.Arguments | 1.0.1 | Runtime | Command-line argument parsing. |
| NuciDAL | 4.1.0 | Runtime | JSON file repositories and data access abstractions. |

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [ .NET ](https://github.com/hmlendea/polyglot-language-validator/blob/master/.github/workflows/dotnet.yml) | Build and test validation | Checks out the repository, restores dependencies, builds the solution, and runs tests. |

## 🗂️ Project Structure

The repository contains the application, its unit tests, and supporting documentation.

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `PolyglotLanguageValidator/` | .NET executable | CLI application. |
| `PolyglotLanguageValidator.UnitTests/` | .NET test project | Unit tests for the application. |

### Directories

| Directory | Purpose |
|-----------|---------|
| `PolyglotLanguageValidator/Configuration/` | Command-line input configuration. |
| `PolyglotLanguageValidator/Models/` | Application and Polyglot data models. |
| `PolyglotLanguageValidator/Service/` | Parsing, declension-building, and validation services. |
| `docs/` | Usage, architecture, development, and reference documentation. |

## 🏗️ Architecture

Polyglot Language Validator uses a layered architecture with dependency injection, repository-based JSON access, dictionary parsing, declension building, and sentence validation.

See the [architecture documentation](ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📚 Documentation

The repository contains detailed documentation for usage, configuration, architecture, development, and troubleshooting.

| Resource | Description |
|----------|-------------|
| [Quick start](docs/quick-start.md) | Build, prepare input files, run validation, and run tests. |
| [Configuration](docs/configuration.md) | CLI configuration and input formats. |
| [API usage examples](docs/api-usage-examples.md) | Extended usage examples. |
| [Documentation index](docs/INDEX.md) | Index of repository documentation. |

## 🩺 Troubleshooting

| Symptom | Probable Cause | Resolution |
|---------|----------------|------------|
| Input file not found | A supplied path is incorrect or the file is missing. | Verify the paths and use absolute paths if necessary. |
| Generated sentence mismatch | Dictionary rules or expected sentence data differ from the generated output. | Check the dictionary, dimensions, declension rules, and expected sentence. |
| `Sequence contains no matching element` | The dictionary lacks an expected class, word, or dimension node. | Verify the corresponding Polyglot dictionary entries. |
| Dependencies are unavailable | Packages have not been restored. | Run `dotnet restore PolyglotLanguageValidator.slnx`. |

## ❓ FAQ

See the [Frequently Asked Questions](./docs/faq.md) for responses to frequently raised inquiries.

## 🩺 Troubleshooting

See [Troubleshooting Guide](./docs/troubleshooting.md) for diagnostic procedures and resolutions to recurring problems.

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/polyglot-language-validator/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](./CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🔗 Related Projects

- [Polyglot](https://github.com/draquet/polyglot): The Polyglot application project providing the dictionary format and language-generation context.
- [Polyglot Language Validate](https://github.com/hmlendea/polyglot-language-validate): The GitHub Action for running this validator.

## 🔒 Security

Polyglot Language Validator runs locally and processes user-supplied input files; its security scope includes local XML/JSON parsing and command-line handling.

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/polyglot-language-validator/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for additional information.
