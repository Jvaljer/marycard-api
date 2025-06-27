using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using AuthModule.Domain;
using AuthModule.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthModule.Application.Commands;


public sealed record SignInCommand : ICommand<SignInModel>
{
    public required string Identifier { get; init; }
    public required string Password { get; init; }
}

internal sealed class SignInHandler(SignInManager<User> signInManager, UserManager<User> userManager) : ICommandHandler<SignInCommand, SignInModel>
{
    public async Task<Result<SignInModel, MessagingError>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Identifier);

        if (user is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "User not found.");
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, true, true);

        if (result.Succeeded)
        {
            var roles = await userManager.GetRolesAsync(user);
            return SignInModel.ValidCredentials(roles);
        }

        if (result.IsLockedOut)
        {
            return SignInModel.AccountLocked();
        }

        var passwordIsValid = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (passwordIsValid.Succeeded && !user.EmailConfirmed)
        {
            return SignInModel.EmailNotVerified();
        }

        return SignInModel.InvalidCredentials();
    }
}
