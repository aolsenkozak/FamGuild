using FamGuild.Core.Application.Common;
using FamGuild.Core.Domain.Common.ResultPattern;
using FamGuild.Core.Domain.Treasury;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.Core.Application.Treasury.RecurringTransactions.Get;

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