# Security

## Threat model

**Attack surface**: Local file reading only. No network, no user input processing beyond file paths, no authentication, no authorization.

## Data handling

| Data type | Source | Sensitivity | Protection |
|---|---|---|---|
| Dictionary XML | Local file | Low (conlang data) | File permissions |
| Words JSON | Local file | Low | File permissions |
| Sentences JSON | Local file | Low | File permissions |
| Generated output | Memory | Low | Stdout only |

## No secrets

- No API keys, tokens, passwords, connection strings
- No certificate handling
- No encryption/decryption

## Input validation

- File existence checked by `File.ReadAllText` (throws if missing)
- XML/JSON parsing validates structure (throws on malformed)
- No path traversal protection — uses provided paths directly
- **Mitigation**: Run with least privilege; don't process untrusted files

## Dependency security

- All NuGet packages from nuget.org
- Pinned versions (no floating)
- Run `dotnet list package --vulnerable --include-transitive` periodically

## Execution context

- Runs as current user
- No elevation required
- No system modification
- Single-process, single-threaded

## Output safety

- Validation failures printed to stdout
- No structured data that could be injection vector
- Exit codes only (0/1)

## Known limitations

1. **Path traversal** — `--dictionary ../../etc/passwd` would read arbitrary files
2. **Large files** — No size limits; could OOM on huge inputs
3. **XML external entities** — `XmlSerializer` default settings; mitigate by not processing untrusted XML

## Recommendations

- Process only trusted input files
- Run in isolated environment if inputs untrusted
- Monitor dependency vulnerabilities
- Consider adding file size limits if used in automated pipelines