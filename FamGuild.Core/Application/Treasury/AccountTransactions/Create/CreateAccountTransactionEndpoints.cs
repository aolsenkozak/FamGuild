using FamGuild.Core.Application.Common;
using FamGuild.Core.Domain.Common.ResultPattern;

namespace FamGuild.Core.Application.Treasury.AccountTransactions.Create;

public static class CreateAccountTransactionEndpoints
{
    public static void AddCreateAccountTransactionHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateAccountTransactionsCommand, Result<List<Guid>>>, CreateAccountTransactionHandler>();
    }

    public static void RegisterCreateAccountTransactionEndpoints(this WebApplication app)
    {
        app.MapPost("account-transactions", async (
            CreateAccountTransactionsCommand command,
            ICommandHandler<CreateAccountTransactionsCommand, Result<List<Guid>>> handler) =>
        {
            var handlerResult = await handler.HandleAsync(command);

            if (handlerResult.IsFailure) return Results.InternalServerError(handlerResult.Error.Message);
            var ids = handlerResult.Value;
            return Results.Created($"/recurring-transactions/", ids);
        });
    }
}