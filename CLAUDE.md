# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

WhereAndHow is an apartment rental platform. The backend is an ASP.NET Core Web API on .NET 10 and the frontend is Angular 21, served as a SPA from the .NET host. The .NET SDK version is pinned in `global.json` (10.0.0, `rollForward: latestMajor`).

## Commands

### Backend (.NET)

Run from the solution root (`WhereAndHow/`) or the server project directory:

```bash
# Run the API server
dotnet run --project WhereAndHow.Server

# Seed the database
dotnet run --project WhereAndHow.Server seed

# EF Core migrations (run from Infrastructure.Persistence/)
dotnet ef migrations add <MigrationName> --startup-project ../WhereAndHow.Server
dotnet ef database update --startup-project ../WhereAndHow.Server

# Build entire solution
dotnet build WhereAndHow.sln

# Run tests (if any test projects are added)
dotnet test
```

### Frontend (Angular)

Run from `whereandhow.client/`:

```bash
npm install
npm start          # ng serve (dev with proxy)
npm test           # Karma unit tests
npm run build      # production build
```

## Architecture

The solution follows a **Clean Architecture / layered** pattern with four .NET projects and one Angular project:

```
Core.Domain          → domain models, DTOs, enums — no dependencies
Application          → service interfaces only, depends on Core.Domain
Infrastructure.Persistence → EF Core + PostgreSQL repositories, depends on Core.Domain
Infrastructure.Service     → service implementations, depends on Application
WhereAndHow.Server   → ASP.NET Core host; wires everything together, serves Angular SPA
whereandhow.client   → Angular 21 SPA (referenced from the server via whereandhow.client.esproj)
```

**Dependency flow**: `Server` → `Infrastructure.Service` + `Infrastructure.Persistence` → `Application` → `Core.Domain`

Each infrastructure project has a `ConfigurationService.cs` with extension methods (`AddInfrastructurePersistenceService`, `AddInfrastructureService`, `AddInfrastructureWeb`) that are called from `Program.cs` to register their services.

### Backend Key Points

- **ORM**: Entity Framework Core with Npgsql (PostgreSQL). `UserContext` is the DbContext. It uses ASP.NET Identity (`AddIdentity<User, IdentityRole>`).
- **Auth**: JWT Bearer. Config lives in `appsettings.json` under `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`. The server project's `ConfigurationService.cs` sets this up.
- **Repositories**: `IAddressRepository` / `IApartamentRepository` in `Infrastructure.Persistence/Interfaces`, implemented in `Infrastructure.Persistence/Implements`.
- **Services**: Interfaces in `Application/Interfaces`, implementations in `Infrastructure.Service/Services` (Address, Apartament, Auth, Order, User, Partner, EmailSender).
- **Controllers**: `WhereAndHow.Server/Controllers` (Address, Apartment, Authenticate, Order, User, Partner).
- **Rate limiting**: A fixed-window limiter named `partner-request` (3 calls per IP per hour, no queue, 429 on rejection) is configured in `Program.cs` and applied to partner-request endpoints.
- **CORS**: A single permissive policy `MyPolice` (`AllowAnyOrigin/Method/Header`) is applied globally.
- **File uploads**: Stored under `wwwroot/uploads/`, served as static files at `/uploads`.
- **Seeding**: `Infrastructure.Persistence/Seed/SeedData.cs`, triggered by `dotnet run seed` (the `seed` arg short-circuits `Program.cs` before the web host starts).

### Frontend Key Points

- **Angular structure**: NgModule-based (not standalone) — `app.module.ts` declares components and `app-routing.module.ts` defines routes. Components in `src/app/componetes/` (note the typo — matches the actual folder name), services in `src/app/services/`, DTO/model interfaces in `src/app/Model/`, route guard in `src/app/guard/`, interceptors in `src/app/interceptors/`.
- **Auth flow**: `auth.service.ts` handles login/register. `auth-interceptor.interceptor.ts` attaches JWT to outgoing requests. `auth-guard.guard.ts` protects routes.
- **External services**: Firebase (`@angular/fire`) is used alongside the .NET backend.
- **UI**: Bootstrap 5 + ngx-bootstrap.
- **SPA proxy**: The server uses `Microsoft.AspNetCore.SpaProxy` — `dotnet run --project WhereAndHow.Server` auto-launches `npm start` (Angular dev server on `localhost:4200`) in dev.

## Configuration

Database connection string and JWT settings are in `WhereAndHow.Server/appsettings.json`. For local dev, PostgreSQL is expected at `localhost:5432` with database `WhereAndHow`.

Override sensitive values in `appsettings.Development.json` or via environment variables/user secrets — do not commit credentials.
