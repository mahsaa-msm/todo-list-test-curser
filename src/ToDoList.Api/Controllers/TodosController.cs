using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Contracts.Todos.Commands.Create;
using ToDoList.Contracts.Todos.Commands.Delete;
using ToDoList.Contracts.Todos.Commands.UpdateStatus;
using ToDoList.Contracts.Todos.Queries.GetMine;

namespace ToDoList.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/todos")]
public sealed class TodosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateTodoDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateTodoCommand command, CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        var location = $"{Request.Path}/{id}";
        return Created(location, new CreateTodoDto { Id = id });
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TodoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TodoDto>>> ListMine(CancellationToken ct)
    {
        var items = await mediator.Send(new GetMyTodosQuery(), ct);
        return Ok(items);
    }

    [HttpPatch("{id:long}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateTodoStatusCommand command, CancellationToken ct)
    {
        await mediator.Send(command with { TodoId = id }, ct);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await mediator.Send(new DeleteTodoCommand(id), ct);
        return NoContent();
    }
}
