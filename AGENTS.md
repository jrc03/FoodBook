# AGENTS.md

Architectural rules for FoodBook. Read this before adding or changing code.

The README is operational only; do not duplicate rationale there.

---

## 1. Dependencies point inward only

```
WebApi ──► Infrastructure ──► Application ──► Domain
WebApi ─────────────────────► Application ──► Domain
```

| Project | May reference |
|---|---|
| `FoodBook.Domain` | **nothing**. No project, no NuGet package. |
| `FoodBook.Application` | `Domain` only. |
| `FoodBook.Infrastructure` | `Application`, `Domain`. |
| `FoodBook.WebApi` | `Application`, `Infrastructure`. |
| `FoodBook.Tests` | all four. |

**The interface lives in the layer that uses it; the implementation in the layer
that provides it.** So `IFoodRepository` goes in `Application/Interfaces/` and
its EF implementation in `Infrastructure/Persistence/Repositories/`. Never inject
a concrete Infrastructure type into an Application service.

`tests/FoodBook.Tests/Architecture/` is reserved for a test that asserts the
table above automatically. It is currently empty, so the rule is enforced by
review only.

---

## 2. Domain model: anemic, on purpose

Entities in `Domain/Entities/` are plain data holders — public getters/setters,
no behavior. **All business rules live in `Application/Services/`.**

Anemic models dominate real .NET codebases and play well with EF Core and
AutoMapper. Strict DDD (aggregate roots, value objects, invariants in
constructors) needs base classes, guarded constructors, and a mapping strategy
per entity before any feature exists. That tradeoff is not worth it here.

Do not add behavior to entities, and do not introduce `Primitives/`,
`Aggregates/`, `ValueObjects/`, or `Events/` without a concrete requirement.

**The line that must hold:** rules stay in `Application/Services/` and never
leak up into `WebApi/Controllers/`.

---

## 3. Persistence: EF Core in-memory, permanently

`FoodBookDbContext` is registered with `UseInMemoryDatabase("FoodBookDb")` in
`Infrastructure/DependencyInjection.cs`. This is the final choice, not a
placeholder — there is no migration path to a real database.

So: data resets on every restart and nothing is shared across processes. Write
plain LINQ; do not reach for raw SQL, transactions, or connection strings.

`OnModelCreating` already calls `ApplyConfigurationsFromAssembly`, so
`IEntityTypeConfiguration<T>` classes in `Persistence/Configurations/` are
picked up automatically.

---

## 4. Packages

Versions live in `Directory.Packages.props` (Central Package Management);
`TargetFramework`, `Nullable`, and `ImplicitUsings` come from
`Directory.Build.props`. **Never put a `Version` attribute on a
`PackageReference`** — add a `PackageVersion` entry instead.

### AutoMapper is pinned to 14.0.0 — do not upgrade casually

- 14.0.0 is the last MIT release. **15.0.0+ requires a paid license key**, and
  the `AddAutoMapper` overloads change shape.
- 14.0.0 carries NU1903 / CVE-2026-32933 (uncontrolled recursion DoS, CVSS 7.5).
  It needs a ~25,000-level-deep object graph, so it is not reachable at
  coursework scale. The patch, 15.1.1+, is license-gated.

Upgrading means registering for Lucky Penny's free Community License (student
coursework) and passing `cfg.LicenseKey` in
`Application/DependencyInjection.cs`.

FluentValidation 12.1.1 is still Apache 2.0 — free, no constraints.
`FluentValidation.DependencyInjectionExtensions` is required for
`AddValidatorsFromAssembly`.

Deliberately not used: **FluentAssertions** (v8+ is commercial; use xUnit's
`Assert`), **Docker** (nothing to containerize with no database),
**`.editorconfig`** (single-author project).

---

## 5. Where code goes

| Writing... | Goes in |
|---|---|
| A property describing a thing | `Domain/Entities/` |
| A business rule | `Application/Services/` |
| HTTP request/response shape | `Application/DTOs/` |
| DTO ↔ entity conversion | `Application/Mapping/` (AutoMapper `Profile`) |
| "Is this incoming request well-formed?" | `Application/Validators/` |
| `DbSet`, query, or save | `Infrastructure/Persistence/` |
| External service client | `Infrastructure/Services/` |
| Verb, route, status code | `WebApi/Controllers/` |

Controllers stay thin: no business rules, no direct `DbContext` access.

Do not duplicate validation. A validator that already rejects `PrepMinutes < 0`
at the boundary means the service does not re-check it.

---

## 6. Conventions

- Keep a `.gitkeep` in every scaffolded folder; delete it when the first real
  file lands.
- Register DI per layer — `AddApplicationLayer()`, `AddInfrastructureLayer()`,
  `AddWebApiLayer()`. Never register in `Program.cs`.
- Leave `public partial class Program;` at the end of `Program.cs`. Integration
  tests need it for `WebApplicationFactory<Program>`.
- Add endpoints to `FoodBook.http` as you build them.
