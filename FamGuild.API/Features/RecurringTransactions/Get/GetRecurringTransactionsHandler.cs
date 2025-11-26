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
        List<RecurringTransaction> recurringTransactions = [];

        if (command.Id != Guid.Empty)
        {
            var recurringItem = await dbContext.RecurringTransactions
                .FindAsync(command.Id, ct);

            if (recurringItem == null)
            {
                var error = new Error("NotFound", "Recurring item not found.");
                return Result.Failure<List<RecurringTransaction>>(error);
            }

            recurringTransactions.Add(recurringItem);
            return Result.Success(recurringTransactions);
        }

        recurringTransactions.AddRange(dbContext.RecurringTransactions.AsNoTracking().ToList());
        return Result.Success(recurringTransactions);
    }
}