using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions;

public class AccountTransactionGenerationService(FamGuildDbContext dbContext)
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
        var lastOccurredDateTime = await dbContext.AccountTransactions
            .AsNoTracking()
            .Where(x => x.RecurringTransactionId == recurringTransaction.Id)
            .MaxAsync(x => x.DateOccurred, ct);

        var newOccurranceDate = DateOnly.FromDateTime(lastOccurredDateTime);

        List<AccountTransactionDto> accountTransactionDtos = [];

        while (newOccurranceDate <= endDate)
        {
            newOccurranceDate = recurringTransaction.Recurrence.Frequency switch
            {
                Frequencies.Weekly => newOccurranceDate.AddDays(7),
                Frequencies.BiWeekly => newOccurranceDate.AddDays(14),
                Frequencies.Monthly => newOccurranceDate.AddMonths(1),
                Frequencies.Quarterly => newOccurranceDate.AddMonths(3),
                Frequencies.Yearly => newOccurranceDate.AddYears(1),
                _ => DateOnly.MinValue
            };

            if (startDate <= newOccurranceDate && newOccurranceDate <= endDate)
            {
                var accountTransactionResult = AccountTransaction.CreateFromRecurringTransaction(recurringTransaction,
                    newOccurranceDate.ToDateTime(TimeOnly.MinValue), AccountTransactionStatus.Pending);

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