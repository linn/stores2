# Copilot Instructions for `stores2`

## Purpose and scope
- `stores2` is a stores/logistics forms + reporting application over LinnApps Oracle data.
- Keep changes minimal and local to the requested feature/fix.
- Prefer existing project patterns and `Linn.Common.*` abstractions over introducing new frameworks.

## Solution architecture (high-level)
Use this dependency flow when making changes:
1. `Service.Host` (ASP.NET Core host, auth, static files, endpoint wiring)
2. `Service` (`*Module` endpoint classes)
3. `Facade` (use-case orchestration, resource shaping, result wrappers)
4. `Domain.LinnApps` (business logic)
5. `Persistence.LinnApps` + `Proxy` (Oracle EF repositories, external HTTP/SP integrations)

Cross-cutting registration is in `IoC` (`ServiceExtensions`, `PersistenceExtensions`, `HandlerExtensions`).

## Important project conventions
- Endpoints are implemented as module classes implementing `IModule` (minimal API style), not MVC controllers.
- View-serving endpoints often return `Index.cshtml` via `ViewResponse` + negotiator.
- API responses usually use `res.Negotiate(...)` with handlers from `Linn.Common.Service`.
- Prefer adding dependencies through `IoC` extension methods rather than wiring ad hoc in host startup.

## Auth and host behavior
- Host entrypoint: `src/Service.Host/Program.cs`.
- Auth is dual-scheme via `MultiAuthHandler`:
  - legacy auth (`AddLinnAuthentication` / bearer)
  - Cognito JWT bearer (`cognito-provider`)
- `MultiAuthHandler` routes based on JWT issuer.
- Frontend/static content is served from `/stores2/build` with environment-specific physical path.

## `Linn.Common.*` usage (salient abstractions)
These are foundational in this repository:
- `Linn.Common.Service`
  - endpoint module mapping + negotiation/handlers (`IHandler`, `JsonResultHandler`, csv/stream handlers)
- `Linn.Common.Facade`
  - facade service contracts/results (`IAsyncFacadeService`, `IAsyncQueryFacadeService`, result types)
- `Linn.Common.Persistence` / `.EntityFramework`
  - repositories (`IRepository`, `IQueryRepository`, `ISingleRecordRepository`) + `TransactionManager`
- `Linn.Common.Reporting`
  - report layouts/models (`ResultsModel`, `SimpleGridLayout`, axis/value models)
- `Linn.Common.Proxy`
  - `IRestClient` wrappers for external services
- `Linn.Common.Rendering` + `Linn.Common.Pdf`
  - html template rendering + pdf generation services
- `Linn.Common.Configuration`
  - environment/config lookup via `ConfigurationManager.Configuration[...]`

Prefer these over custom alternatives unless there is a clear gap.

## Data access and domain notes
- Oracle EF Core mappings are centralized in `src/Persistence.LinnApps/ServiceDbContext.cs`.
- Repository pattern is mixed:
  - generic EF repositories for simple CRUD
  - custom repositories for complex queries/procedures
- Domain services in `Domain.LinnApps` contain core logic; facade/services should stay orchestration-focused.

## Testing patterns
### Unit tests
- NUnit + NSubstitute + FluentAssertions.
- Pattern: per-feature folder with `ContextBase` constructing SUT and substitutes.
- Prefer focused behavior tests (`WhenX...`) and assert report grid positions where applicable.

### Integration tests
- Build lightweight `HttpClient` via test helper (`TestClient.With<Module>(...)`).
- Register only needed services/handlers/routing for module under test.
- Use test DB context (`TestServiceDbContext`) and clear data in `[TearDown]`.

## Build/test/deploy workflow assumptions
- CI uses `.NET 10` and Node `24`.
- Scripts: `scripts/install.sh`, `scripts/test.sh`, `scripts/package.sh`, `scripts/deploy.sh`.
- Known repository drift to watch:
  - docs/scripts still reference `Messaging.Host`/`Messaging.Tests`, but these projects are not currently present in this workspace.
  - prefer checking actual solution/projects before acting on script README assumptions.

## Configuration and secrets handling
- Runtime config is env-driven (`DATABASE_*`, auth, proxy, pdf, etc.).
- Never commit real credentials or tokens; treat local `config.env` as sensitive.
- If editing config templates/examples, preserve key names expected by `ApplicationSettings` and startup.

## Change guidance for future Copilot sessions
- Before coding, locate the existing layer where equivalent behavior already exists and extend that pattern.
- For new endpoints:
  - add to a `Service/Modules/*Module.cs`
  - route to existing/new facade interface
  - register services/builders/handlers in `IoC`
  - add unit/integration tests in matching style
- Keep auth behavior consistent with current dual-scheme model.
- Validate with relevant tests first, then broader build/test if needed.

## Repository-Level Copilot Instructions
- Follow the established architecture for all new features and fixes, ensuring alignment with the existing dependency flow.
- Utilize `Linn.Common` abstractions to maintain consistency and reduce redundancy in code.
- Adhere to the testing patterns outlined, ensuring both unit and integration tests are comprehensive and follow the established structure.
- Maintain project conventions, particularly regarding endpoint implementation and dependency injection practices.
