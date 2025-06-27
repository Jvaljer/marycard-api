using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using AuthModule.Domain;
using AuthModule.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthModule.Application.Query;

public sealed record GetUserByIdQuery : IQuery<UserModel>
{
    public required Guid UserId { get; init; }
}

internal sealed class GetUserByIdHandler(UserManager<User> userManager) : IQueryHandler<GetUserByIdQuery, UserModel>
{
    public async Task<Result<UserModel, MessagingError>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "User not found.");
        }

        var roles = await userManager.GetRolesAsync(user);
        return Mapper.UserToUserModel(user, roles);
    }
}