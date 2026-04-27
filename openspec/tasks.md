# Implementation Tasks - TodoList Backend

## Phase 1 - Bootstrap
- [ ] Create ASP.NET Core Web API project (`net10.0`).
- [ ] Add NuGet packages:
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Tools`
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
- [ ] Configure app settings for SQL Server and JWT secrets.

## Phase 2 - Data Layer
- [ ] Create entities: `User`, `Todo`.
- [ ] Create `AppDbContext` and entity configurations.
- [ ] Add unique constraint for `Username`.
- [ ] Add migration and update SQL Server database.

## Phase 3 - Authentication
- [ ] Implement register endpoint.
- [ ] Implement password hashing and verification.
- [ ] Implement login endpoint and JWT token generation.
- [ ] Configure JWT bearer authentication middleware in `Program.cs`.

## Phase 4 - Todo API
- [ ] Implement create todo endpoint (`POST /api/todos`).
- [ ] Implement list my todos endpoint (`GET /api/todos`).
- [ ] Implement update completion endpoint (`PATCH /api/todos/{id}/status`).
- [ ] Implement delete todo endpoint (`DELETE /api/todos/{id}`).
- [ ] Enforce ownership checks for all todo operations.

## Phase 5 - Validation and Errors
- [ ] Add request DTOs for auth and todo APIs.
- [ ] Add FluentValidation package and register validators in DI container.
- [ ] Implement FluentValidation validators for auth and todo request DTOs.
- [ ] Keep only essential data annotations where needed; move business input validation to FluentValidation.
- [ ] Standardize API error responses and status codes.

## Phase 6 - Quality and Readiness
- [ ] Add Swagger/OpenAPI support for API documentation.
- [ ] Add basic integration tests for:
  - auth register/login
  - unauthorized todo access
  - CRUD on own todos
  - forbidden/cross-user scenarios
- [ ] Verify acceptance criteria from PRD one by one.

## Definition of Done
- [ ] All PRD acceptance criteria are satisfied.
- [ ] Todos are persisted in SQL Server.
- [ ] JWT protection is active on todo endpoints.
- [ ] User can only access their own data.
- [ ] Project is clean and ready for portfolio/demo.
