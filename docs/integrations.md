# Integrations

## External dependencies (NuGet)

| Package | Version | Integration type |
|---|---|---|
| Microsoft.Extensions.DependencyInjection | 10.0.5 | DI container |
| NuciCLI.Arguments | 1.0.1 | CLI parsing |
| NuciDAL | 3.0.1 | JSON repository |

## Microsoft.Extensions.DependencyInjection

**Purpose**: Service registration and resolution.

**Usage**:
```csharp
var services = new ServiceCollection();
services.AddSingleton<ILanguageParser, LanguageParser>();
services.AddSingleton<IDeclesionBuilder, DeclesionBuilder>();
services.AddSingleton<ILanguageValidator, LanguageValidator>();
services.AddSingleton<IFileRepository<Word>, JsonRepository<Word>>();
services.AddSingleton<IFileRepository<Sentence>, JsonRepository<Sentence>>();
var provider = services.BuildServiceProvider();
```

**Configuration**: None (programmatic only).

## NuciCLI.Arguments

**Purpose**: Parse command-line arguments into `InputSettings`.

**Usage**:
```csharp
var settings = new InputSettings(args);
```

**Argument mapping**:
- `--dictionary` / `-d` → `DictionaryPath`
- `--words` / `-w` → `WordsPath`
- `--sentences` / `-s` → `SentencesPath`

**Behaviour**: Throws on missing required args.

## NuciDAL

**Purpose**: File-based JSON repository implementing `IRepository<T>`.

**Interfaces used**:
- `IFileRepository<Word>` (extends `IRepository<Word>`)
- `IFileRepository<Sentence>` (extends `IRepository<Sentence>`)

**Implementation**: `JsonRepository<T>`

**File format**: JSON array of entities with `Id` property (from `EntityBase`).

**Operations used**:
- `GetAll()` — returns all entities
- `Get(id)` — returns single entity by ID

## No other integrations

- **No HTTP clients** — No REST/gRPC calls
- **No databases** — No EF Core, Dapper, ADO.NET
- **No message queues** — No RabbitMQ, Kafka, Azure Service Bus
- **No cloud services** — No AWS, Azure, GCP SDKs
- **No authentication** — No Identity, OAuth, JWT
- **No monitoring** — No OpenTelemetry, Prometheus, Application Insights
- **No configuration providers** — No appsettings.json, environment variables, Azure Key Vault

## Extension points for integrations

### Custom dictionary format

Implement `ILanguageParser`:
```csharp
public class DatabaseLanguageParser : ILanguageParser
{
    // Load from database instead of XML
}
```

Register:
```csharp
services.AddSingleton<ILanguageParser, DatabaseLanguageParser>();
```

### Custom storage

Implement `IFileRepository<T>`:
```csharp
public class SqlWordRepository : IFileRepository<Word>
{
    // Load from SQL database
}
```

Register:
```csharp
services.AddSingleton<IFileRepository<Word>, SqlWordRepository>();
```

### Custom validation logic

Implement `ILanguageValidator`:
```csharp
public class LenientLanguageValidator : ILanguageValidator
{
    // Different validation rules
}
```

Register:
```csharp
services.AddSingleton<ILanguageValidator, LenientLanguageValidator>();
```

### Custom rule engine

Implement `IDeclesionBuilder`:
```csharp
public class AdvancedDeclesionBuilder : IDeclesionBuilder
{
    // Different rule application logic
}
```

Register:
```csharp
services.AddSingleton<IDeclesionBuilder, AdvancedDeclesionBuilder>();
```

## Version compatibility

| Dependency | Target framework | Notes |
|---|---|---|
| Microsoft.Extensions.DependencyInjection | net10.0 | Matches project TFM |
| NuciCLI.Arguments | net10.0 | Compatible |
| NuciDAL | net10.0 | Compatible |

All dependencies target .NET 10.0 or are compatible with it.