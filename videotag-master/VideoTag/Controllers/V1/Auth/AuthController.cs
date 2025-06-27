using AryDotNet.Authorization;
using AryDotNet.Messaging;
using AryDotNet.Result;
using AuthModule.Application.Commands;
using AuthModule.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoTag.Controllers.V2.Auth;

namespace VideoTag.Controllers.V1.Auth;

[Authorize]
[ApiController]
[Route("v1/auth")]
public sealed class AuthController(ISender sender, IAuthContext authContext) : OldRestController()
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SignInCommand
        {
            Identifier = request.Identifier,
            Password = request.Password
        }, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SignOutCommand(), cancellationToken);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpGet("check")]
    public Task<IActionResult> CheckAuth()
    {
        return Task.FromResult(HandleResult(Result<bool, MessagingError>.Ok(authContext.IsUserConnected())));
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetConnectedUser(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserByIdQuery
        {
            UserId = authContext.ConnectedUserId()
        }, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserByIdQuery
        {
            UserId = userId
        }, cancellationToken);
        return HandleResult(result);
    }
}
