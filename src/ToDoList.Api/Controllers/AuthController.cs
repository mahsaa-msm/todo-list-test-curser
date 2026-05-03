using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Contracts.Auth.Commands.Login;
using ToDoList.Contracts.Auth.Commands.Register;

namespace ToDoList.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var userId = await mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, new RegisterResponse { UserId = userId });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var token = await mediator.Send(command, ct);
        return Ok(new LoginResponse { AccessToken = token });
    }
}
