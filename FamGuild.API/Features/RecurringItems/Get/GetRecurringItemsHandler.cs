using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.RecurringItems.Get;

public class GetRecurringItemsHandler(FamGuildDbContext dbContext)
    : ICommandHandler<GetRecurringItemsCommand, Result<List<RecurringTransaction>>>
{
    
    public async Task<Result<List<RecurringTransaction>>> HandleAsync(GetRecurringItemsCommand command, 
        CancellationToken ct = default)
    {
        List<RecurringTransaction> recurringItems = [];
        
        if (command.Id != Guid.Empty)
        {
            var recurringItem = await dbContext.RecurringItems
                .FindAsync(command.Id, ct);

            if (recurringItem == null)
            {
                var error = new Error("NotFound", "Recurring item not found.");
                return Result.Failure<List<RecurringTransaction>>(error);
            }
            
            recurringItems.Add(recurringItem);
            return Result.Success(recurringItems);
        }

        recurringItems.AddRange(dbContext.RecurringItems.AsNoTracking().ToList());
        return Result.Success(recurringItems);
        
    }
}