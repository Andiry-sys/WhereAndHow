# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

WhereAndHow is an apartment rental platform. The backend is ASP.NET Core Web API and the frontend is Angular 15, served as a SPA from the .NET host.

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
whereandhow.client   → Angular 15 SPA
```

**Dependency flow**: `Server` → `Infrastructure.Service` + `Infrastructure.Persistence` → `Application` → `Core.Domain`

Each infrastructure project has a `ConfigurationService.cs` with extension methods (`AddInfrastructurePersistenceService`, `AddInfrastructureService`, `AddInfrastructureWeb`) that are called from `Program.cs` to register their services.

### Backend Key Points

- **ORM**: Entity Framework Core with Npgsql (PostgreSQL). `UserContext` is the DbContext. It uses ASP.NET Identity (`AddIdentity<User, IdentityRole>`).
- **Auth**: JWT Bearer. Config lives in `appsettings.json` under `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`. The server project's `ConfigurationService.cs` sets this up.
- **Repositories**: `IAddressRepository` / `IApartamentRepository` in `Infrastructure.Persistence/Interfaces`, implemented in `Infrastructure.Persistence/Implements`.
- **Services**: Interfaces in `Application/Interfaces`, implementations in `Infrastructure.Service/Services` (Address, Apartament, Auth, Order, User, EmailSender).
- **File uploads**: Stored under `wwwroot/uploads/`, served as static files at `/uploads`.
- **Seeding**: `Infrastructure.Persistence/Seed/SeedData.cs`, triggered by `dotnet run seed`.

### Frontend Key Points

- **Angular structure**: Components in `src/app/componetes/` (note the typo — matches the actual folder name), services in `src/app/services/`.
- **Auth flow**: `auth.service.ts` handles login/register. `auth-interceptor.interceptor.ts` attaches JWT to outgoing requests. `auth-guard.guard.ts` protects routes.
- **External services**: Firebase (`@angular/fire`) is used alongside the .NET backend.
- **UI**: Bootstrap 5 + ngx-bootstrap.

## Configuration

Database connection string and JWT settings are in `WhereAndHow.Server/appsettings.json`. For local dev, PostgreSQL is expected at `localhost:5432` with database `WhereAndHow`.

Override sensitive values in `appsettings.Development.json` or via environment variables/user secrets — do not commit credentials.
