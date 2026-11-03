# Build and deployment

## Build

### Prerequisites

- .NET 10.0 SDK
- No additional system dependencies

### Commands

```bash
# Build solution
dotnet build PolyglotLanguageValidator.slnx

# Build specific project
dotnet build PolyglotLangageValidator/PolyglotLangageValidator.csproj

# Build tests
dotnet build PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj
```

### Build outputs

- `PolyglotLangageValidator/bin/Debug/net10.0/PolyglotLangageValidator` — Executable
- `PolyglotLangageValidator/bin/Debug/net10.0/*.dll` — Dependencies
- `PolyglotLanguageValidator.UnitTests/bin/Debug/net10.0/*.dll` — Test assemblies

## Test

```bash
# Run all tests
dotnet test PolyglotLanguageValidator.slnx

# Run specific test project
dotnet test PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Test framework

- **NUnit 4.x** — Test framework
- **Moq** — Mocking
- **NUnit3TestAdapter** — VS Code / CLI integration
- **Microsoft.NET.Test.Sdk** — Test host

## Release

### Release script

```bash
./release.sh [version]
```

The `release.sh` script downloads and executes a shared release script from `hmlendea/dotnet-release-scripts`.

### Release process

1. Script fetches `https://raw.githubusercontent.com/hmlendea/dotnet-release-scripts/refs/heads/master/dotnet/10.0.sh`
2. Executes with provided version argument
3. Handles versioning, tagging, packaging, publishing

### Versioning

- Uses semantic versioning
- Version passed as argument to release script
- Git tags created for releases

## Deployment

### Distribution

- Single-file executable (via `dotnet publish`)
- Self-contained or framework-dependent
- Cross-platform (Linux, Windows, macOS)

### Publish command

```bash
# Framework-dependent (requires .NET 10 runtime)
dotnet publish PolyglotLangageValidator/PolyglotLangageValidator.csproj -c Release -o ./publish

# Self-contained (includes runtime)
dotnet publish PolyglotLangageValidator/PolyglotLangageValidator.csproj -c Release -r linux-x64 --self-contained true -o ./publish/linux-x64
dotnet publish PolyglotLangageValidator/PolyglotLangageValidator.csproj -c Release -r win-x64 --self-contained true -o ./publish/win-x64
dotnet publish PolyglotLangageValidator/PolyglotLangageValidator.csproj -c Release -r osx-x64 --self-contained true -o ./publish/osx-x64
```

### Runtime requirements

- **Framework-dependent**: .NET 10.0 runtime
- **Self-contained**: No runtime required (larger binary)

## CI/CD

### GitHub Actions

Workflow: `.github/workflows/dotnet.yml`

Runs on:
- Push to main/master
- Pull requests

Steps:
1. Checkout
2. Setup .NET 10.0
3. Restore dependencies
4. Build solution
5. Run tests

### No deployment automation

- No automated deployment to package registries
- No Docker image publishing
- Manual release via `release.sh`

## Environments

### Development

- `dotnet run` for quick iteration
- Debug builds in `bin/Debug/`
- Hot reload not supported (console app)

### Production

- Release builds via `dotnet publish`
- Self-contained executables for distribution
- No configuration files — all inputs via CLI

## Dependencies

### Production

| Package | Version | Purpose |
|---|---|---|
| Microsoft.Extensions.DependencyInjection | 10.0.5 | DI container |
| NuciCLI.Arguments | 1.0.1 | CLI argument parsing |
| NuciDAL | 3.0.1 | JSON repository, EntityBase |

### Test

| Package | Version | Purpose |
|---|---|---|
| Microsoft.NET.Test.Sdk | 17.13.0 | Test host |
| NUnit | 4.3.2 | Test framework |
| NUnit3TestAdapter | 5.0.0 | Adapter |
| Moq | 4.20.72 | Mocking |

## Project configuration

### PolyglotLangageValidator.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>PolyglotLanguageValidator</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.5" />
    <PackageReference Include="NuciCLI.Arguments" Version="1.0.1" />
    <PackageReference Include="NuciDAL" Version="3.0.1" />
  </ItemGroup>
</Project>
```

### PolyglotLanguageValidator.UnitTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <RootNamespace>PolyglotLanguageValidator.UnitTests</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="Moq" Version="4.20.72" />
    <PackageReference Include="NUnit" Version="4.3.2" />
    <PackageReference Include="NUnit3TestAdapter" Version="5.0.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="../PolyglotLangageValidator/PolyglotLangageValidator.csproj" />
  </ItemGroup>
</Project>
```

### Solution file (PolyglotLanguageValidator.slnx)

```xml
<Solution>
  <Project Path="PolyglotLangageValidator/PolyglotLangageValidator.csproj" />
  <Project Path="PolyglotLanguageValidator.UnitTests/PolyglotLanguageValidator.UnitTests.csproj" />
</Solution>
```

## Known issues

1. **Project folder typo** — `PolyglotLangageValidator` (missing 'u' in Language) — preserved for compatibility
2. **No nullable enable** — Projects don't have `<Nullable>enable</Nullable>` in csproj
3. **No implicit usings** — `<ImplicitUsings>disable</ImplicitUsings>` not set (uses SDK defaults)