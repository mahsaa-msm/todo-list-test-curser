using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Contracts.Todos;
using ToDoList.Application.Todos.Commands;
using ToDoList.Application.Todos.Queries;

namespace ToDoList.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class TodosController(
    IMediator mediator,
    IValidator<CreateTodoRequest> createValidator,
    IValidator<UpdateTodoStatusRequest> updateValidator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);
        var created = await mediator.Send(new CreateTodoCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetMyTodos), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyTodos(CancellationToken cancellationToken)
    {
        var todos = await mediator.Send(new GetMyTodosQuery(), cancellationToken);
        return Ok(todos);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTodoStatusRequest request, CancellationToken cancellationToken)
    {
        await updateValidator.ValidateAndThrowAsync(request, cancellationToken);
        var result = await mediator.Send(new UpdateTodoStatusCommand(id, request), cancellationToken);
        return result switch
        {
            TodoMutationResult.Succeeded => NoContent(),
            TodoMutationResult.NotFound => NotFound(),
            TodoMutationResult.Forbidden => Forbid(),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTodoCommand(id), cancellationToken);
        return result switch
        {
            TodoMutationResult.Succeeded => NoContent(),
            TodoMutationResult.NotFound => NotFound(),
            TodoMutationResult.Forbidden => Forbid(),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
