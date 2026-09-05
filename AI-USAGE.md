# AI Usage Disclosure

## Tool

Claude Code (Anthropic's CLI), model Claude Sonnet 5 (`claude-sonnet-5`).

## Bugs it helped find and fix

A few real bugs came up during development that AI assistance helped catch and resolve:

- A `TaskCanceledException` I was seeing while debugging was diagnosed as `HttpContext.RequestAborted` firing because my HTTP client was timing out while I was paused on a breakpoint — not a bug in the code itself, but managed to prevent diagnosing a non-issue.
- FluentValidation validators weren't being registered — `AddValidatorsFromAssemblyContaining` was being passed the result of calling `.GetType()` on an `Assembly` object instead of a marker type, so it was scanning the wrong assembly.
- In `SearchProduct`, I was calling `.Result` on the search-products task inside a `.Select`. It was safe here since `Task.WhenAll` had already awaited it, but I cleaned it up anyway — moved the `await` into a local variable before the `.Select` so the closure doesn't touch `.Result` at all.

## Simple/mechanical tasks I offloaded

For a handful of small, well-defined changes, I directed exactly what I wanted and had it write the code rather than typing it myself:

- Looking up `JsonException` and `IOException` for what to implement in `GetSearchProducts`, so it returns an empty result instead of failing outright when the search data file is corrupted or unavailable.
- Adding simple fields like `Title` for ProductDTOs and some minor field updates and additions.
- Wiring the (already-present but unused) `appsettings.json` `DataFiles` section through to `ProductRepository` using the standard `IOptions<T>` configuration pattern, replacing hardcoded path constants.
- Generating a `.http` file with sample requests for manual testing.

## Review feedback I asked for

Separately from writing code, I used it as a reviewer on decisions I'd already made or was making myself:

- Asked it to compare the finished solution against the challenge README to check what was covered, and to work through some of the README's more open-ended wording (e.g. what "especially price, currency, stock" implies about other overlapping fields, and what counts as "graceful" handling of bad data) to help me decide where to draw the line.
- Asked it to review the DI service-lifetime choices (singleton/scoped/transient) across the solution. Everything turned out to be correct, but it was worth the check.
- Asked it to review my `Task.WhenAll` parallelization change in `ProductService` for code quality.

## Review and verification

Everything above was built `dotnet build` and exercised manually over HTTP before I accepted it, including deliberately corrupting each data file in turn to confirm the intended status code and fallback behavior actually happened, not just that the code compiled.
