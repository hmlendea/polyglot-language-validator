# API usage examples

## Basic validation

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  --dictionary dictionary.xml \
  --words words.json \
  --sentences sentences.json
```

## With absolute paths

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  --dictionary /home/user/conlang/dictionary.xml \
  --words /home/user/conlang/words.json \
  --sentences /home/user/conlang/sentences.json
```

## Using aliases

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  -d dictionary.xml \
  -w words.json \
  -s sentences.json
```

## Mixed aliases and full names

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  -d dictionary.xml \
  --words words.json \
  -s sentences.json
```

## Equals syntax

```bash
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  --dictionary=dictionary.xml \
  --words=words.json \
  --sentences=sentences.json
```

## CI/CD pipeline

```yaml
# .github/workflows/validate.yml
name: Validate Conlang

on: [push, pull_request]

jobs:
  validate:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      - name: Build
        run: dotnet build PolyglotLanguageValidator.slnx
      - name: Validate
        run: |
          dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
            --dictionary test-data/dictionary.xml \
            --words test-data/words.json \
            --sentences test-data/sentences.json
```

## Batch validation (multiple dictionaries)

```bash
#!/bin/bash
# validate-all.sh

for dict in dictionaries/*.xml; do
  name=$(basename "$dict" .xml)
  echo "Validating $name..."
  dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
    --dictionary "$dict" \
    --words "words/$name.json" \
    --sentences "sentences/$name.json" || exit 1
done
echo "All validations passed"
```

## Development workflow

```bash
# 1. Make changes to dictionary.xml
# 2. Quick test
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  -d dictionary.xml -w words.json -s sentences.json

# 3. Run tests
dotnet test

# 4. If tests pass, commit
```

## Debugging rule application

```bash
# Add temporary debug output to DeclesionBuilder.GetWord, then:
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- \
  -d dictionary.xml -w words.json -s sentences.json 2>&1 | head -50
```

## Self-contained executable

```bash
# Publish
dotnet publish PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -c Release -r linux-x64 --self-contained true -o ./publish

# Run published
./publish/PolyglotLanguageValidator \
  --dictionary dictionary.xml \
  --words words.json \
  --sentences sentences.json
```

## Docker

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PolyglotLanguageValidator/PolyglotLanguageValidator.csproj", "PolyglotLanguageValidator/"]
RUN dotnet restore "PolyglotLanguageValidator/PolyglotLanguageValidator.csproj"
COPY . .
WORKDIR "/src/PolyglotLanguageValidator"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PolyglotLanguageValidator.dll"]
```

```bash
# Build
docker build -t polyglot-validator .

# Run
docker run --rm -v $(pwd)/data:/data polyglot-validator \
  --dictionary /data/dictionary.xml \
  --words /data/words.json \
  --sentences /data/sentences.json
```

## PowerShell (Windows)

```powershell
dotnet run --project PolyglotLanguageValidator/PolyglotLanguageValidator.csproj -- `
  --dictionary dictionary.xml `
  --words words.json `
  --sentences sentences.json

# Check exit code
if ($LASTEXITCODE -eq 0) { Write-Host "Validation passed" } else { Write-Host "Validation failed" }
```

## With environment-specific files

```bash
# Development
dotnet run -- -d dict.dev.xml -w words.dev.json -s sentences.dev.json

# Production
dotnet run -- -d dict.prod.xml -w words.prod.json -s sentences.prod.json
```