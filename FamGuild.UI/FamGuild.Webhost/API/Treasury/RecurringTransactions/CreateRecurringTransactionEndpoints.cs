using FamGuild.Core.Application.Common;
using FamGuild.Core.Application.Treasury.RecurringTransactions.Create;
using FamGuild.Core.Domain.Common.ResultPattern;

namespace FamGuild.UI.API.Treasury.RecurringTransactions;

public static class CreateRecurringTransactionEndpoints
{
    public static void AddCreateRecurringTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateRecurringTransactionCommand, Result<Guid>>, CreateRecurringTransactionHandler>();
    }

    public static void RegisterCreateRecurringTransactionEndpoints(this WebApplication app)
    {
        app.MapPost("recurring-transactions", async (
            CreateRecurringTransactionCommand command,
            ICommandHandler<CreateRecurringTransactionCommand, Result<Guid>> handler) =>
        {
            var handlerResult = await handler.HandleAsync(command);

            if (handlerResult.IsFailure) return Results.InternalServerError(handlerResult.Error.Message);
            var id = handlerResult.Value;
            return Results.Created($"/recurring-transactions/{id}", id);
        });
    }
}