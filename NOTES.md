# Submission Notes

## Project structure

I split the solution into four projects, each with a single responsibility:

- **`PerfectDraft.Product.Shared`**: the DTOs (`ProductDTO`, `ProductSkuDTO`, `ProductSearchTermDTO`) and their FluentValidation validators, plus the raw upstream data shapes (`MagentoProductModel`, `SearchProductModel`). These shapes live here because `Service`'s repository interface needs them without depending on `Infrastructure`.
- **`PerfectDraft.Product.Infrastructure`**: reads the upstream data and implements `Service`'s `IProductRepository`. `IJsonFileLoader`/`JsonFileLoader` deserializes a JSON file into a typed collection, and `ProductRepository` exposes the Magento and search datasets. `IJsonFileLoader` wraps `System.IO.Abstractions`' `IFileSystem` rather than calling `File.OpenRead` directly, so file access can be mocked in tests instead of hitting disk.
- **`PerfectDraft.Product.Service`**: `ProductService`, the aggregation logic that combines the two upstream sources into a single `ProductDTO`, and the `IProductRepository` interface it depends on. This is where the "Magento wins on conflicts" rule lives.
- **`src/PerfectDraft.Product.Api`**: the ASP.NET Core host, containing `ProductController`, `Program.cs`, and configuration. It's also the one place that wires the concrete `ProductRepository` to the interface `Service` owns.

`Shared` doesn't depend on anything. `Service` depends only on `Shared`. `Infrastructure` depends on `Service`, to implement its repository interface, and `Shared`, for the data shapes. `Api` depends on all three, since it's the one place that needs to see both the interface and its concrete implementation, to wire them together.

## Design decisions

**DTO shape.** `ProductDTO` is: `Sku`, `Name`, `Title`, `Url`, `Price`, `Currency`, `Stock`, `InStock`. `Name` always comes from Magento; `Title` is the search index's display text, included as enrichment alongside the image since the README's "indexed display content" is left open-ended. `Sku` is wrapped in a small value-object record (`ProductSkuDTO`) rather than passed around as a bare string. A SKU is an identity, not just text, and treating it as a plain string risks losing that, letting it collapse into an incidental string field instead of a value with its own meaning.

**Architecture.** This is a layered architecture. Rather than adopting the full Clean Architecture pattern, a project with a single domain, Product, doesn't have the complexity to justify a separate project holding domain entities, business rules, and every interface the app depends on, I borrowed one specific technique from it at the one place an external dependency actually crosses a boundary. `IProductRepository` is defined in `Service`, not `Infrastructure`, so `Service` can build and be tested without `Infrastructure` existing at all, and `Infrastructure` has to conform to the contract `Service` defines, rather than the other way round.

**Magento as source of truth.** `ProductService` builds the response starting from the Magento product and only reaches into the search dataset to fill in `Title` and `Url` (falling back to Magento's `Name` and a default image respectively when there's no matching search entry). This also means a product that only exists in the search index can never be returned, since the aggregation is driven off the Magento list, not a union of both.

**Multi-term search.** The README just specifies `search={term}`; I split the query on whitespace (trimming each piece) and match if a product's name contains *any* of the resulting terms, so a multi-word search like "Stella Artois" works properly instead of only matching an exact substring, returning every match rather than just the first. An empty match set returns `200` with an empty array, since an empty result is still a successful search, not a missing resource.

**Data-loading resilience.** The README only calls out status codes for "success and missing," and only asks for missing/partial data to be handled gracefully in general terms. I went a step further on both: the search dataset is enrichment-only, so if `search-products.json` fails to load or parse, the repository catches that and returns an empty set, so every product still comes back with sensible defaults, just without enrichment, rather than the request failing outright. The Magento dataset has no such fallback: if it fails to load, that propagates as a genuine `500` (via ASP.NET Core's exception-handling middleware) instead of being collapsed into a misleading `404`, since there's no sensible default for the source of truth.

**Validation.** `ProductSkuValidator` and `ProductSearchValidator` are FluentValidation validators run explicitly in the controller before hitting the service, returning a `422` response with `ValidationProblemDetails` on failure for both endpoints. SKU validation checks a `P` or `M` prefix (case-insensitive) and a length range based on the sample data's SKU format. I initially only allowed `P`, then extended it to `M` after noticing `data/magento-products.json` includes an `M100` SKU that the original rule would have rejected.

## Assumptions

- The README says Magento wins "especially for price, currency, stock." Those fields all have one objectively correct value, so a conflict genuinely needs a winner. I checked whether `name` and `title` had that same property, and they don't: `Name` is Magento's canonical product name and `Title` is the search index's display text, two different pieces of content, not two versions of the same fact. So instead of picking a winner, I kept both as separate fields, which keeps more information than picking one over the other would. `Title` still falls back to Magento's `Name` when there's no search match, the same fallback pattern used for the image URL.
- A single malformed record inside an otherwise-valid data file still fails the whole read for that file. Resilience is handled at the file level (missing or corrupt file), not the individual-record level.

## Not completed

- Full unit test coverage. The core paths are tested (product found or not found, search matches or no matches, and the controller's status codes), but under time pressure I didn't get to every edge case, for example multiple simultaneous matches in `SearchProduct` or dedicated tests for `ProductSkuValidator`'s prefix and length rules. I know these gaps exist and what they'd take to close, I just ran out of time to write them.
- No caching of the parsed JSON. Both files are re-read and re-parsed from disk on every request.
- No pagination or rate limiting on the Search endpoint. It returns every match in one response and has no limit on how often it can be called.

## What I'd do next with more time

- Finish the missing tests: multiple search matches, multi-word search, tests for the SKU validator, a real test for the Magento-wins-on-conflict rule in `GetProduct`, and tests for the controller's success and Search paths.
- Add caching. Right now every request re-reads and re-parses both JSON files from scratch. If these were a real database and search index, I'd cache products by SKU and search results by search term, with a short expiry, clearing the cache when something changes rather than trying to update it in place.
- Add resilience for the two data sources, but only as much as makes sense here. The files are read from local disk, so they're never going to hang. Adding timeouts and retries for that would be solving a problem that doesn't exist. If this were a real call to Magento and a real search index, I'd set a timeout once, in the shared HTTP client setup, rather than on every call, and add retries and a circuit breaker on top of that.
- Add Swagger, so the API is easy to explore manually, and double check both endpoints return errors in the same shape.
