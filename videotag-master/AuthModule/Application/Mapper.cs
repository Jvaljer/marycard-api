using AuthModule.Domain;
using AuthModule.Model;

namespace AuthModule.Application;

internal static class Mapper
{
    public static UserModel UserToUserModel(User user, ICollection<string> roles) => new()
    {
        Id = user.Id,
        Identifier = user.UserName ?? "",
        Email = user.Email,
        Roles = roles
    };
}