using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Features.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions.Get;

public class GetAccountTransactionsHandler(FamGuildDbContext dbContext, 
    IAccountTransactionGenerationService accountTransactionGenerationService) :
    IQueryHandler<GetAccountTransactionsQuery, Result<List<AccountTransactionDto>>>
{
    public async Task<Result<List<AccountTransactionDto>>> HandleAsync(GetAccountTransactionsQuery query, CancellationToken ct = default)
    {
        var recurringItemTransactionsResult = await accountTransactionGenerationService
            .CreateAccountTransactionsForDateRange(query.StartDate, query.EndDate, ct);

        if (recurringItemTransactionsResult.IsFailure)
        {
            return Result.Failure<List<AccountTransactionDto>>(recurringItemTransactionsResult.Error);
        }
        
        var accountTransactionDtos = recurringItemTransactionsResult.Value;
        
       var startDateWithTime = query.StartDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
       var endDateWithTime = query.EndDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();
       
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