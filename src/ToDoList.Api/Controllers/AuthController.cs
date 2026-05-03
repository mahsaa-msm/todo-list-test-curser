using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Auth.Commands;
using ToDoList.Application.Contracts.Auth;

namespace ToDoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    IMediator mediator,
    IValidator<RegisterRequest> registerValidator,
    IValidator<LoginRequest> loginValidator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await registerValidator.ValidateAndThrowAsync(request, cancellationToken);
        var outcome = await mediator.Send(new RegisterCommand(request), cancellationToken);
        return outcome switch
        {
            RegisterSucceeded s => CreatedAtAction(nameof(Register), new { id = s.Id }, new { s.Id, Username = s.Username }),
            UsernameAlreadyRegistered => Conflict(new { message = "Username already exists." }),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        await loginValidator.ValidateAndThrowAsync(request, cancellationToken);
        var outcome = await mediator.Send(new LoginCommand(request), cancellationToken);
        return outcome switch
        {
            LoginSucceeded s => Ok(new { accessToken = s.AccessToken }),
            InvalidCredentials => Unauthorized(new { message = "Invalid credentials." }),
            _ => Problem(statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}
