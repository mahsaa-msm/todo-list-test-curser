namespace ToDoList.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string CreateAccessToken(long userId, string username);
}
