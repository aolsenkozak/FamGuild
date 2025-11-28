using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamGuild.API.Features.AccountTransactions.Get;

public static class GetAccountTransactionsEndpoints
{
    public static void AddGetRecurringTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<GetAccountTransactionsQuery, Result<List<AccountTransaction>>>,
                GetAccountTransactionsHandler>();
    }

    public static void RegisterGetAccountTransactionEndpoints(this WebApplication app)
    {
        app.MapGet("account-transactions/{id}", async (
            [FromRoute] Guid id,
            [FromServices] GetByIdQueryHandler<AccountTransaction> handler) =>
        {
            var query = new GetByIdQuery<AccountTransaction>(id);
            var handlerResult = await handler.HandleAsync(query);

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

        app.MapGet("account-transactions/", async (
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromServices] IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransaction>>> handler) =>
        {
            var query = new GetAccountTransactionsQuery(startDate, endDate);
            var handlerResult = await handler.HandleAsync(query);

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