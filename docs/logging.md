# Logging

## Overview

Polyglot Language Validator uses **no structured logging framework**. It is a simple CLI tool that writes validation results directly to `stdout` and errors to `stderr`. There are no log files, no log levels, no correlation IDs, and no external logging sinks.

## Output streams

### Standard output (stdout)

- **Validation failures only** — One line per mismatched sentence
- **Format**: `The actual value '{actual}' does not match the expected value '{expected}'.`
- **No output on success** — Silent when all sentences match

### Standard error (stderr)

- **Unhandled exceptions** — Full stack trace printed by .NET runtime
- **Argument parsing errors** — From NuciCLI.Arguments library

## Log levels

Not applicable. No log levels exist.

## Structured logging

Not applicable. No structured logging.

## Correlation IDs

Not applicable. No request correlation.

## Sensitive data handling

- **No secrets logged** — Tool processes only dictionary, words, and sentences files
- **No PII** — Input files contain constructed language data only
- **No credentials** — No authentication or authorization in the tool

## Performance impact

- **Zero overhead** — No logging framework initialized
- **Synchronous I/O** — Console writes block but are minimal (failure-only)

## Configuration

No logging configuration exists. Cannot be configured.

## Log retention

Not applicable. No log files created.

## Log aggregation

Not applicable. No log aggregation.

## Audit logging

Not applicable. No audit trail.

## Compliance

Not applicable. No regulatory logging requirements.

## Observability

### Health checks

None. Tool runs once and exits.

### Metrics

None emitted.

### Tracing

None. No distributed tracing.

### Debugging

- Run with `dotnet run -- --dictionary dict.xml --words words.json --sentences sentences.json`
- Add `Console.WriteLine` temporarily in source for debugging
- Use debugger (VS Code launch.json) for step-through

## Future considerations

If logging becomes necessary:
1. Add `Microsoft.Extensions.Logging` + `Microsoft.Extensions.Logging.Console`
2. Inject `ILogger<T>` into services
3. Replace `Console.WriteLine` with `logger.LogError/Information`
4. Add `--verbosity` CLI flag
5. Consider structured JSON output for CI/CD parsing