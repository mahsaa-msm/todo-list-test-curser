using ToDoList.Domain.Entities;

namespace ToDoList.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
