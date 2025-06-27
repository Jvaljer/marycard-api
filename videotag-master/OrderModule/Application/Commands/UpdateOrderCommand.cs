using System.Net;
using AryDotNet.Messaging;
using AryDotNet.Result;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderModule.Domain;
using OrderModule.Infrastructure;

namespace OrderModule.Application.Commands;

public sealed record UpdateOrderCommand : ICommand
{
    public required Guid OrderId { get; init; }
    public required string? Note { get; init; }
    public required OrderState State { get; init; }
    public required string? CustomerPhone { get; init; }
    public required string? CustomerFirstName { get; init; }
    public required string? CustomerLastName { get; init; }
    public required string? CustomerEmail { get; init; }
    public required string? ContactEmail { get; init; }
}

internal sealed class UpdateOrderCommandHandler(OrderDbContext dbContext) : ICommandHandler<UpdateOrderCommand>
{
    public async Task<Result<MessagingError>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Set<Order>()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
        {
            return new MessagingError(HttpStatusCode.NotFound, "Order not found");
        }

        order.Note = request.Note;
        order.NoteNormalized = request.Note?.ToUpperInvariant();
        order.State = request.State;
        order.CustomerPhone = request.CustomerPhone;
        order.CustomerFirstName = request.CustomerFirstName;
        order.CustomerFirstNameNormalized = request.CustomerFirstName?.ToUpperInvariant();
        order.CustomerLastName = request.CustomerLastName;
        order.CustomerLastNameNormalized = request.CustomerLastName?.ToUpperInvariant();
        order.CustomerEmail = request.CustomerEmail;
        order.CustomerEmailNormalized = request.CustomerEmail?.ToUpperInvariant();
        order.ContactEmail = request.ContactEmail;
        order.ContactEmailNormalized = request.ContactEmail?.ToUpperInvariant();

        dbContext.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<MessagingError>.Ok();
    }
}