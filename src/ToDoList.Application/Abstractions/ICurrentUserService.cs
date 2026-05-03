namespace ToDoList.Application.Abstractions;

public interface ICurrentUserService
{
    long UserId { get; }
}
