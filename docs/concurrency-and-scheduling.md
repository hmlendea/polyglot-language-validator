# Concurrency and scheduling

## Execution model

**Single-threaded, synchronous, run-to-completion.**

```
Main thread
    │
    ├── Parse CLI args
    ├── Build DI container
    ├── Load dictionary (XML)
    ├── Load words (JSON)
    ├── Load sentences (JSON)
    ├── Validate all sentences
    │     └── For each sentence:
    │           ├── Build constructed sentence
    │           └── Compare with expected
    ├── Collect mismatches
    └── Exit (0 or 1)
```

## No concurrency

- No `Task`, `async`, `await`
- No `Thread`, `ThreadPool`, `Parallel`
- No `BackgroundService`, `IHostedService`
- No timers, no schedulers

## No shared state

- All services are singletons but **stateless** (except `LanguageParser` dictionary cache)
- Dictionary loaded once, read-only thereafter
- Repositories read files on each call (no in-memory cache)
- No static mutable fields

## No locking

- No `lock`, `Monitor`, `Mutex`, `Semaphore`
- No `ConcurrentDictionary`, `ConcurrentQueue`
- No `Interlocked`

## Scheduling

- **No scheduling** — Runs once and exits
- **No background jobs** — No periodic work
- **No queue processing** — No message loops

## Implications

### Testing

- Deterministic execution — same input always produces same output
- No flaky tests from race conditions
- No need for synchronization in tests

### Scaling

- **Vertical only** — Faster CPU = faster run
- **No horizontal scaling** — Not designed for parallel execution
- **Batch processing** — Run multiple instances for parallel validation

### Resource usage

- Memory: Dictionary + words + sentences in memory
- CPU: Single core
- I/O: Sequential file reads
- No connection pools, no thread pools

## Future considerations

If concurrency needed:
1. Make `LanguageParser` thread-safe (immutable dictionary → already safe)
2. Add `async` file I/O for large files
3. Use `Parallel.ForEach` for sentence validation (independent)
4. Add `CancellationToken` support
5. Consider `Channels` for producer/consumer if streaming