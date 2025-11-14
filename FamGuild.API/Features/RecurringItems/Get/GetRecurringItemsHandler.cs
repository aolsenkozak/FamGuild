using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.RecurringItems.Get;

public class GetRecurringItemsHandler(FamGuildDbContext dbContext)
    : ICommandHandler<GetRecurringItemsCommand, Result<List<RecurringItem>>>
{
    
    public async Task<Result<List<RecurringItem>>> HandleAsync(GetRecurringItemsCommand command, 
        CancellationToken ct = default)
    {
        List<RecurringItem> recurringItems = [];
        
        if (command.Id != Guid.Empty)
        {
            var recurringItem = await dbContext.RecurringItems
                .FindAsync(command.Id, ct);

            if (recurringItem == null)
            {
                var error = new Error("NotFound", "Recurring item not found.");
                return Result.Failure<List<RecurringItem>>(error);
            }
            
            recurringItems.Add(recurringItem);
            return Result.Success(recurringItems);
        }

        recurringItems.AddRange(dbContext.RecurringItems.AsNoTracking().ToList());
        return Result.Success(recurringItems);
        
    }
}