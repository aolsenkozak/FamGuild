using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamGuild.API.Features.RecurringItems.Get;

public static class GetRecurringItemEndpoints
{
    public static void AddGetRecurringItemCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<GetRecurringItemsCommand, Result<List<RecurringTransaction>>>, GetRecurringItemsHandler>();
    }
    
    public static void RegisterGetRecurringItemEndpoints(this WebApplication app)
    {
        app.MapGet("recurring-items/{id}", async (
            [FromRoute] Guid? id,
            [FromServices] ICommandHandler<GetRecurringItemsCommand, Result<List<RecurringTransaction>>> handler) =>
        {
            var command = new GetRecurringItemsCommand(id ?? Guid.Empty);
            var handlerResult = await handler.HandleAsync(command);

            return handlerResult switch
            {
                {IsFailure : true, Error.Code : "NotFound"}
                    => Results.NotFound(),
                { IsFailure:true, Error: var error}
                    => Results.InternalServerError(error.Message),
                {Value: var value} 
                    => Results.Ok(value)
            };

        });
        
        app.MapGet("recurring-items/", async (
            [FromServices] ICommandHandler<GetRecurringItemsCommand, Result<List<RecurringTransaction>>> handler) =>
        {
            var command = new GetRecurringItemsCommand(Guid.Empty);
            var handlerResult = await handler.HandleAsync(command);

            return handlerResult switch
            {
                {IsFailure : true, Error.Code : "NotFound"}
                    => Results.NotFound(),
                { IsFailure:true, Error: var error}
                    => Results.InternalServerError(error.Message),
                {Value: var value} 
                    => Results.Ok(value)
            };

        });
    }
    
}