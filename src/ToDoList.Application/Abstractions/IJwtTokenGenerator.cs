namespace ToDoList.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string CreateAccessToken(int userId, string username);
}
