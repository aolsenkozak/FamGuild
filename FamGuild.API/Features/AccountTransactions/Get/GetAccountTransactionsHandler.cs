using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions.Get;

public class GetAccountTransactionsHandler(FamGuildDbContext dbContext, 
    AccountTransactionGenerationService accountTransactionGenerationService) :
    ICommandHandler<GetAccountTransactionsQuery, Result<List<AccountTransaction>>>
{
    public async Task<Result<List<AccountTransaction>>> HandleAsync(GetAccountTransactionsQuery query, CancellationToken ct = default)
    {
        //first, we want to generate any Account Transactions that are a result of RecurringItems
        List<AccountTransaction> accountTransactions = [];
       
        var recurringItemTransactionsResult = await accountTransactionGenerationService
            .CreateAccountTransactionsForDateRange(query.StartDate, query.EndDate, ct);

        if (recurringItemTransactionsResult.IsFailure)
        {
            return Result.Failure<List<AccountTransaction>>(recurringItemTransactionsResult.Error);
        }
        
        accountTransactions = recurringItemTransactionsResult.Value;
        
       var startDateWithTime = query.StartDate.ToDateTime(TimeOnly.MinValue);
       var endDateWithTime = query.EndDate.ToDateTime(TimeOnly.MaxValue);
       
        //then, we return all Account Transactions for the specified date range
        var existingAccountTransactions = await dbContext.AccountTransactions
            .AsNoTracking()
            .Where(at => startDateWithTime <= at.DateOccurred && at.DateOccurred <= endDateWithTime)
            .ToListAsync(ct);
        
        accountTransactions.AddRange(existingAccountTransactions);
        
        return Result.Success(accountTransactions);
    }

     
}