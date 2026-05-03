namespace ToDoList.Application.Contracts.Auth;

public abstract record RegisterOutcome;

public sealed record RegisterSucceeded(int Id, string Username) : RegisterOutcome;

public sealed record UsernameAlreadyRegistered : RegisterOutcome;

public abstract record LoginOutcome;

public sealed record LoginSucceeded(string AccessToken) : LoginOutcome;

public sealed record InvalidCredentials : LoginOutcome;
