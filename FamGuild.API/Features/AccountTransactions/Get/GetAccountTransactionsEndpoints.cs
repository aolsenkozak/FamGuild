using FamGuild.Domain.Common.ResultPattern;
using FamGuild.API.Features.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamGuild.API.Features.AccountTransactions.Get;

public static class GetAccountTransactionsEndpoints
{
    public static void AddGetAccountTransactionCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IAccountTransactionGenerationService, AccountTransactionGenerationService>();
        
        services
            .AddScoped<IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionDto>>>,
                GetAccountTransactionsHandler>();
    }

    public static void RegisterGetAccountTransactionEndpoints(this WebApplication app)
    {
        app.MapGet("account-transactions/{id}", async (
            [FromRoute] Guid id,
            [FromServices] GetByIdQueryHandler<AccountTransactionDto> handler) =>
        {
            var query = new GetByIdQuery(id);
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
            [FromServices] IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionDto>>> handler) =>
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