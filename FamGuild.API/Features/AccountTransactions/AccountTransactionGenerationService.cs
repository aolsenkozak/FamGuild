using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions;

public class AccountTransactionGenerationService(FamGuildDbContext dbContext) : IAccountTransactionGenerationService
{
    public async Task<Result<List<AccountTransactionDto>>> CreateAccountTransactionsForDateRange(
        DateOnly startDate, DateOnly endDate, CancellationToken ct = default)
    {
        var recurringTransactions = dbContext.RecurringTransactions
            .AsNoTracking()
            .ToList();

        List<AccountTransactionDto> newAccountTransactionDtos = [];

        foreach (var recTransaction in recurringTransactions)
        {
            var result = await CreateAccountTransactionsForRecurringTransactionForDateRange(recTransaction, startDate, endDate,
                ct);
            if (result.IsFailure)
            {
                return Result.Failure<List<AccountTransactionDto>>(result.Error);
            }
            
            newAccountTransactionDtos.AddRange(result.Value);
        }
        
        return Result.Success(newAccountTransactionDtos);
    }

    private async Task<Result<List<AccountTransactionDto>>> CreateAccountTransactionsForRecurringTransactionForDateRange(
        RecurringTransaction recurringTransaction, DateOnly startDate, DateOnly endDate, CancellationToken ct = default)
    {
        List<AccountTransactionDto> accountTransactionDtos = [];
        
        var lastOccurredDateTime = await dbContext.AccountTransactions
            .Where(x => x.RecurringTransactionId == recurringTransaction.Id)
            .Select(x => x.DateOccurred)
            .DefaultIfEmpty()
            .MaxAsync(ct);

        if (lastOccurredDateTime == DateTime.MinValue)
        {
           
        }

        var lastOccurranceDate = lastOccurredDateTime !=  DateTime.MinValue 
            ? DateOnly.FromDateTime(lastOccurredDateTime)
            : recurringTransaction.Recurrence.StartDate ;
        
        while (lastOccurranceDate <= endDate)
        {
            lastOccurranceDate = recurringTransaction.Recurrence.Frequency switch
            {
                Frequencies.Weekly => lastOccurranceDate.AddDays(7),
                Frequencies.BiWeekly => lastOccurranceDate.AddDays(14),
                Frequencies.Monthly => lastOccurranceDate.AddMonths(1),
                Frequencies.Quarterly => lastOccurranceDate.AddMonths(3),
                Frequencies.Yearly => lastOccurranceDate.AddYears(1),
                _ => DateOnly.MinValue
            };

            if (startDate <= lastOccurranceDate && lastOccurranceDate <= endDate)
            {
                var accountTransactionResult = AccountTransaction.CreateFromRecurringTransaction(recurringTransaction,
                    lastOccurranceDate.ToDateTime(TimeOnly.MinValue), AccountTransactionStatus.Pending);

                if (accountTransactionResult.IsFailure)
                {
                    return Result.Failure<List<AccountTransactionDto>>(accountTransactionResult.Error);
                }
                
                accountTransactionDtos.Add(accountTransactionResult.Value.ToDto());
            }
        }
        
        return Result.Success(accountTransactionDtos);
    }


}