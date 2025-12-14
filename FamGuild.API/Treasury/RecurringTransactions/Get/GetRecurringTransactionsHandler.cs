using FamGuild.API.Common;
using FamGuild.API.Common.ResultPattern;
using FamGuild.API.Persistence;
using FamGuild.Shared.Treasury.RecurringTransactions;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Treasury.RecurringTransactions.Get;

public class GetRecurringTransactionsHandler(FamGuildDbContext dbContext)
    : IQueryHandler<GetRecurringTransactionsQuery, Result<List<RecurringTransaction>>>
{
    public async Task<Result<List<RecurringTransaction>>> HandleAsync(GetRecurringTransactionsQuery query,
        CancellationToken ct = default)
    {
        var recurringTransactions = await dbContext.RecurringTransactions
            .AsNoTracking()
            .ToListAsync(ct);
        
        return Result.Success(recurringTransactions);
    }
}