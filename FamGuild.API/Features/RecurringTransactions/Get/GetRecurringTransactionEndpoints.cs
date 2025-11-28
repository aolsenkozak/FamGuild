using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamGuild.API.Features.RecurringTransactions.Get;

public static class GetRecurringTransactionEndpoints
{
    public static void AddGetRecurringTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<GetRecurringTransactionsQuery, Result<List<RecurringTransaction>>>,
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