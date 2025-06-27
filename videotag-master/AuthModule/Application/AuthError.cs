using System.Net;
using AryDotNet.Messaging;

namespace AuthModule.Application;

internal static class AuthError
{
    public static MessagingError EmailAlreadyInUse() => new(HttpStatusCode.Conflict, "Email address is already in use");

    public static MessagingError InvalidEmail(string email) =>
        new(HttpStatusCode.BadRequest, $"{email} is not a valid email address.");

}