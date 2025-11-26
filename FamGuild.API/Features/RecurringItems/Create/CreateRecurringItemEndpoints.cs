using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Features.Common;

namespace FamGuild.API.Features.RecurringItems.Create;

public static class CreateRecurringItemEndpoints
{
    public static void AddCreateRecurringItemCommandHandlerToDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateRecurringItemCommand, Result<Guid>>, CreateRecurringItemHandler>();
    }

    public static void RegisterCreateRecurringItemEndpoints(this WebApplication app)
    {
        app.MapPost("recurring-items", async (
            CreateRecurringItemCommand command,
            ICommandHandler<CreateRecurringItemCommand, Result<Guid>> handler) =>
        {
            var handlerResult = await handler.HandleAsync(command);

            if (handlerResult.IsFailure) return Results.InternalServerError(handlerResult.Error.Message);
            var id = handlerResult.Value;
            return Results.Created($"/recurring-items/{id}", id);
        });
    }
}