using AryDotNet.Messaging;
using AryDotNet.Result;
using AuthModule.Domain;
using Microsoft.AspNetCore.Identity;

namespace AuthModule.Application.Commands;

public sealed record SignOutCommand : ICommand;

internal sealed class SignOutHandler(SignInManager<User> signInManager) : ICommandHandler<SignOutCommand>
{
    public async Task<Result<MessagingError>> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();
        return Result<MessagingError>.Ok();
    }
}