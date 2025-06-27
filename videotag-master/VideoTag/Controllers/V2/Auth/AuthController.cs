using AryDotNet.Authorization;
using AryDotNet.Web;
using AuthModule.Application.Commands;
using AuthModule.Application.Query;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoTag.Controllers.V2.Auth;

[Authorize]
[ApiController]
[Route("v2/auth")]
public sealed class AuthController(ISender sender, IAuthContext authContext) : RestController(sender)
{
    private readonly ISender _sender = sender;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SignInCommand
        {
            Identifier = request.Identifier,
            Password = request.Password
        }, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SignOutCommand(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetConnectedUser(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery
        {
            UserId = authContext.ConnectedUserId()
        }, cancellationToken);
        return HandleResult(result);
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserByIdQuery
        {
            UserId = userId
        }, cancellationToken);
        return HandleResult(result);
    }
}