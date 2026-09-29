# FoodBook

Onion architecture, .NET 9, ASP.NET Core Web API, EF Core in-memory.

## Requirements

- .NET SDK 9.0 or later

There is nothing else to install. No database server, no Docker, no external
services. EF Core runs on the in-memory provider, so the data store is a
process-local in-memory database.

## Run

```bash
dotnet run --project src/FoodBook.WebApi
```

Open the Swagger UI at the URL printed in the console, usually
`http://localhost:5245/swagger`.

## Test

```bash
dotnet test
```

## Layout

```
src/
├── FoodBook.Domain/          entities, enums, exceptions
├── FoodBook.Application/     interfaces, services, DTOs, mapping, validators
├── FoodBook.Infrastructure/  EF Core context, repositories, configurations
└── FoodBook.WebApi/          controllers, filters, Swagger
tests/
└── FoodBook.Tests/           unit, integration, architecture
```

## Data caveat

The in-memory database resets every time the app restarts, and it is not shared
across processes, so it will not survive restarts or scale horizontally.

## Contributing

Read [AGENTS.md](AGENTS.md) before changing anything. It documents the
architecture rules this repo is built on and the constraints each layer has.
