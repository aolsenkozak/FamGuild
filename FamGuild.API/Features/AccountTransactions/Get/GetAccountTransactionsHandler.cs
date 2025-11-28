using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions.Get;

public class GetAccountTransactionsHandler(FamGuildDbContext dbContext, 
    AccountTransactionGenerationService accountTransactionGenerationService) :
    IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionDto>>>
{
    public async Task<Result<List<AccountTransactionDto>>> HandleAsync(GetAccountTransactionsQuery query, CancellationToken ct = default)
    {
        //first, we want to generate any Account Transactions that are a result of RecurringItems
        List<AccountTransactionDto> accountTransactionDtos = [];
       
        var recurringItemTransactionsResult = await accountTransactionGenerationService
            .CreateAccountTransactionsForDateRange(query.StartDate, query.EndDate, ct);

        if (recurringItemTransactionsResult.IsFailure)
        {
            return Result.Failure<List<AccountTransactionDto>>(recurringItemTransactionsResult.Error);
        }
        
        accountTransactionDtos = recurringItemTransactionsResult.Value;
        
       var startDateWithTime = query.StartDate.ToDateTime(TimeOnly.MinValue);
       var endDateWithTime = query.EndDate.ToDateTime(TimeOnly.MaxValue);
       
        var existingAccountTransactionDtos = await dbContext.AccountTransactions
            .AsNoTracking()
            .Where(at => startDateWithTime <= at.DateOccurred && at.DateOccurred <= endDateWithTime)
            .Select(at => new AccountTransactionDto(
                at.Id,
                at.Classification,
                at.Name,
                at.Amount,
                at.Category,
                at.DateOccurred,
                at.Status,
                at.RecurringTransactionId
                ))
            .ToListAsync(ct);
        
        accountTransactionDtos.AddRange(existingAccountTransactionDtos);
            
        return Result.Success(accountTransactionDtos);
    }

     
}