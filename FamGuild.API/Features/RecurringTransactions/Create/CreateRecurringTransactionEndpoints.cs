using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Features.Common;

namespace FamGuild.API.Features.RecurringTransactions.Create;

public static class CreateRecurringTransactionEndpoints
{
    public static void AddCreateRecurringTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateRecurringTransactionCommand, Result<Guid>>, CreateRecurringTransactionHandler>();
    }

    public static void RegisterCreateRecurringTransactionEndpoints(this WebApplication app)
    {
        app.MapPost("recurring-items", async (
            CreateRecurringTransactionCommand command,
            ICommandHandler<CreateRecurringTransactionCommand, Result<Guid>> handler) =>
        {
            var handlerResult = await handler.HandleAsync(command);

            if (handlerResult.IsFailure) return Results.InternalServerError(handlerResult.Error.Message);
            var id = handlerResult.Value;
            return Results.Created($"/recurring-items/{id}", id);
        });
    }
}