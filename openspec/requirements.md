# Requirements Spec - TodoList Backend

## Context
This spec is derived from `Docs/PRD.md` and targets a backend-only product using:
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

No frontend is included in scope.

## Functional Requirements

### FR-1: User Registration
- System must provide an endpoint for user registration with `username` and `password`.
- `username` must be unique.
- Password must never be stored in plain text and must be persisted as a secure hash.

### FR-2: User Login
- System must provide an endpoint for login with `username` and `password`.
- On successful login, system must return a JWT access token.
- On invalid credentials, system must return unauthorized response.

### FR-3: Auth Protection
- All Todo endpoints must require a valid JWT token.
- Requests without token or with invalid token must be rejected.

### FR-4: Create Todo
- Authenticated user must be able to create a todo with `title`.
- `title` cannot be null, empty, or whitespace.
- New todo must be linked to the authenticated user.

### FR-5: List User Todos
- Authenticated user must be able to retrieve only their own todos.
- Todos from other users must never be returned.

### FR-6: Update Todo Completion Status
- Authenticated user must be able to toggle `isCompleted` for their own todo.
- User must not be able to update todos that belong to another user.

### FR-7: Delete Todo
- Authenticated user must be able to delete their own todo.
- User must not be able to delete todos that belong to another user.

## Data Requirements

### User
- `Id` (int, PK)
- `Username` (nvarchar, unique, required)
- `PasswordHash` (nvarchar, required)

### Todo
- `Id` (int, PK)
- `UserId` (int, FK -> User.Id, required)
- `Title` (nvarchar, required)
- `IsCompleted` (bit, required, default false)
- `CreatedAt` (datetime2, required, default UTC now)

## Non-Functional Requirements
- API should follow REST conventions and return meaningful HTTP status codes.
- Database provider must be SQL Server.
- API must be clean and interview/resume friendly (simple, readable, testable structure).

## Out of Scope
- Frontend/UI
- Notifications
- Categories, priorities, or other advanced todo features
