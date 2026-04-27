# Design Spec - TodoList Backend

## High-Level Architecture
- **API Layer**: ASP.NET Core controllers expose auth and todo endpoints.
- **Application Layer**: Services contain business rules and ownership checks.
- **Persistence Layer**: EF Core DbContext and entities mapped to SQL Server.
- **Security Layer**: JWT token generation + JWT bearer authentication middleware.

## Project Structure (Suggested)
- `src/TodoList.Api`
  - `Controllers`
    - `AuthController.cs`
    - `TodosController.cs`
  - `DTOs`
    - `Auth`
    - `Todos`
  - `Entities`
    - `User.cs`
    - `Todo.cs`
  - `Data`
    - `AppDbContext.cs`
  - `Services`
    - `AuthService.cs`
    - `TodoService.cs`
  - `Security`
    - `JwtTokenGenerator.cs`
  - `Program.cs`

## Domain Model

### User Entity
- `Id: int`
- `Username: string`
- `PasswordHash: string`
- `Todos: ICollection<Todo>`

### Todo Entity
- `Id: int`
- `UserId: int`
- `Title: string`
- `IsCompleted: bool`
- `CreatedAt: DateTime`
- `User: User`

## API Endpoints

### Auth
- `POST /api/auth/register`
  - Request: `{ "username": "string", "password": "string" }`
  - Responses: `201 Created`, `400 BadRequest`, `409 Conflict`

- `POST /api/auth/login`
  - Request: `{ "username": "string", "password": "string" }`
  - Responses: `200 OK` with JWT token, `401 Unauthorized`

### Todos (JWT Required)
- `POST /api/todos`
  - Request: `{ "title": "string" }`
  - Response: `201 Created`

- `GET /api/todos`
  - Response: `200 OK` with current user's todos

- `PATCH /api/todos/{id}/status`
  - Request: `{ "isCompleted": true|false }`
  - Responses: `200 OK`, `404 NotFound`, `403 Forbidden`

- `DELETE /api/todos/{id}`
  - Responses: `204 NoContent`, `404 NotFound`, `403 Forbidden`

## Authorization Rules
- Todo resource access is scoped by authenticated user ID from JWT claim.
- Any Todo mutation/read must confirm `todo.UserId == currentUserId`.
- Cross-user access returns forbidden or not found based on API policy.

## Validation Rules
- `Username`: required, trimmed, unique.
- `Password`: required (minimum length can be configured, e.g. 6+).
- `Title`: required, cannot be empty or whitespace.

## Error Handling
- Use consistent error response format:
  - `code`
  - `message`
  - optional `details`
- Return proper status codes for validation/auth/ownership failures.

## Persistence Design
- EF Core migrations manage schema evolution.
- SQL Server connection string via configuration.
- Recommended indexes:
  - unique index on `Users.Username`
  - index on `Todos.UserId`
  - optional index on `Todos.CreatedAt`

## Security Notes
- Use strong password hashing (`PasswordHasher<TUser>` or BCrypt/Argon2 package).
- JWT must include user ID and username claims.
- Keep JWT signing key in secure configuration (environment or secrets).
