using FamGuild.API.Common;
using FamGuild.API.Common.ResultPattern;
using FamGuild.Shared.Common;
using FamGuild.Shared.Treasury.RecurringTransactions;
using Microsoft.AspNetCore.Mvc;

namespace FamGuild.API.Treasury.RecurringTransactions.Get;

public static class GetRecurringTransactionEndpoints
{
    public static void AddGetRecurringTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped<IQueryHandler<GetRecurringTransactionsQuery, Result<List<RecurringTransaction>>>,
                GetRecurringTransactionsHandler>();
    }

    public static void RegisterGetRecurringTransactionEndpoints(this WebApplication app)
    {
        app.MapGet("recurring-transactions/{id}", async (
            [FromRoute] Guid id,
            [FromServices] GetByIdQueryHandler<RecurringTransaction> handler) =>
        {
            var command = new GetByIdQuery(id);
            var handlerResult = await handler.HandleAsync(command);

            return handlerResult switch
            {
                { IsFailure : true, Error.Code : "NotFound" }
                    => Results.NotFound(),
                { IsFailure: true, Error: var error }
                    => Results.InternalServerError(error.Message),
                { Value: var value }
                    => Results.Ok(value)
            };
        });

        app.MapGet("recurring-transactions/", async (
            [FromServices] IQueryHandler<GetRecurringTransactionsQuery, Result<List<RecurringTransaction>>> handler) =>
        {
            var command = new GetRecurringTransactionsQuery();
            var handlerResult = await handler.HandleAsync(command);

            return handlerResult switch
            {
                { IsFailure : true, Error.Code : "NotFound" }
                    => Results.NotFound(),
                { IsFailure: true, Error: var error }
                    => Results.InternalServerError(error.Message),
                { Value: var value }
                    => Results.Ok(value)
            };
        });
    }
}