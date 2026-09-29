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

### AutoMapper is pinned to 15.1.3 — do not downgrade

- 14.0.0 was the last MIT release and carries **CVE-2026-32933** (uncontrolled
  recursion DoS, CVSS 7.5), which NuGet flags as NU1903 on every restore. 15.1.1
  is the fix; we sit on 15.1.3.
- **15.0.0+ is license-gated**, but enforcement is log-only: a missing key logs
  one WARNING under category `LuckyPennySoftware.AutoMapper.License`. Nothing
  degrades, there is no license server, and no feature is disabled. **Do not
  paper over this by downgrading to 14.0.0 to silence the warning.**
- 15.x changed `AddAutoMapper`: every overload now takes a config action first,
  which is why the call reads `AddAutoMapper(_ => { }, assembly)`. The empty
  action is required, not dead code — do not remove it.
- The key is picked up automatically from `AUTOMAPPER_LICENSE_KEY` or
  `LUCKYPENNY_LICENSE_KEY` when set in the environment, so no code change is
  needed. Lucky Penny offers a **free Community License for student
  coursework**. For a production key, set `cfg.LicenseKey` in
  `Application/DependencyInjection.cs` — never commit the value.
- 16.x also patches the CVE and supports net9.0, but it drags
  `Microsoft.Extensions.* 10.0.0` into a net9.0 app that already resolves those
  from the ASP.NET 9 shared framework. 15.1.3 depends on 8.0.0 instead, which
  stays inside the major range. Revisit when the project moves to net10.

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

---

## 7. Git workflow

`develop` is the integration branch. `main` only ever holds promoted, working
releases.

| Branch | Rule |
|---|---|
| `develop` | Everyone lands here, through a PR. Never push to it directly. |
| `main` | Promoted from `develop` only. Never commit to it directly. |
| `feature/...` | One per person, per slice. Yours alone. |

### Naming

`feature/<person>-<slice>` — e.g. `feature/jrc-users-auth`. The person prefix
keeps ownership readable in `git branch -a` and stops two people from landing on
the same generic name, like `feature/users`.

### Opening one

Branch from an up-to-date `develop`, and push the new branch to `origin`
immediately — even while it is still empty. A branch that only exists on your
laptop is invisible to the rest of the team, so nobody knows to stay off the
files you are about to touch.

### Opening a PR

Open a PR into `develop` as soon as a slice is coherent — not one commit at a
time, and not after a month. A PR is ready when:

- it does one thing (RF01 end to end, not "user stuff"),
- `dotnet test` passes locally,
- you are stopping for the day.

Frequency is the whole point. A long-lived branch is exactly what creates the
conflicts you are trying to avoid: the longer it lives, the further it drifts
from `develop` and the bigger the merge gets. Small PRs stay trivial to review.

### Staying current

Rebase onto `origin/develop` at the start of each work session and again before
pushing — rebase, not merge, so your branch history stays linear and readable.
Doing that on a private branch you own is safe and costs nobody a merge commit.

**Never force-push to `develop` or `main`.** Someone else's commits sit on top
of them, and force-pushing is how work disappears.

### Before merging

- One reviewer checks the PR against the rules in this file.
- Prefer squash-merge into `develop`, so its history reads as a list of
  finished slices rather than a diary. Do this now, while `main` and `develop`
  are still the same commit — once they diverge, `develop` history is fixed.
- If a PR is red, fix it on the same branch and push again. Never merge red and
  repair it later: `develop` is where everyone starts, so a broken `develop`
  blocks every other person immediately.
