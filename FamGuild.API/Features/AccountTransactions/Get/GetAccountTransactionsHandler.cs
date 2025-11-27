using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions.Get;

public class GetAccountTransactionsHandler(FamGuildDbContext dbContext, 
    AccountTransactionGenerationService accountTransactionGenerationService) :
    ICommandHandler<GetAccountTransactionsCommand, Result<List<AccountTransaction>>>
{
    public async Task<Result<List<AccountTransaction>>> HandleAsync(GetAccountTransactionsCommand command, CancellationToken ct = default)
    {
        //first, we want to generate any Account Transactions that are a result of RecurringItems
        
       await accountTransactionGenerationService.CreateAccountTransactionsForDateRange(command.StartDate, command.EndDate, ct);
        
       var startDateWithTime = command.StartDate.ToDateTime(TimeOnly.MinValue);
       var endDateWithTime = command.EndDate.ToDateTime(TimeOnly.MaxValue);
       
        //then, we return all Account Transactions for the specified date range
        var accountTransactions = await dbContext.AccountTransactions
            .AsNoTracking()
            .Where(at => startDateWithTime <= at.DateOccurred && at.DateOccurred <= endDateWithTime)
            .ToListAsync(ct);
        
        return Result.Success(accountTransactions);
    }
}