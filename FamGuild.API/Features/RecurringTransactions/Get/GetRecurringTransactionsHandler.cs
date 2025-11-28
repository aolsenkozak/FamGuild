using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.RecurringTransactions.Get;

public class GetRecurringTransactionsHandler(FamGuildDbContext dbContext)
    : ICommandHandler<GetRecurringTransactionsCommand, Result<List<RecurringTransaction>>>
{
    public async Task<Result<List<RecurringTransaction>>> HandleAsync(GetRecurringTransactionsCommand command,
        CancellationToken ct = default)
    {
        var recurringTransactions = await dbContext.RecurringTransactions
            .AsNoTracking()
            .ToListAsync(ct);
        
        return Result.Success(recurringTransactions);
    }
}