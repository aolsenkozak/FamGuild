using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;
using FamGuild.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.API.Features.AccountTransactions;

public class AccountTransactionGenerationService(FamGuildDbContext dbContext)
{
    public async Task<Result> CreateAccountTransactionsForDateRange(
        DateOnly startDate, DateOnly endDate, CancellationToken ct = default)
    {
        var recurringTransactions = dbContext.RecurringTransactions
            .AsNoTracking()
            .ToList();

        foreach (var recTransaction in recurringTransactions)
        {
            var result = await CreateAccountTransactionsForRecurringTransactionForDateRange(recTransaction, startDate, endDate);
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }
        }
        
        return Result.Success();
    }

    private async Task<Result> CreateAccountTransactionsForRecurringTransactionForDateRange(
        RecurringTransaction recurringTransaction, DateOnly startDate, DateOnly endDate, CancellationToken ct = default)
    {
        var lastOccurredDateTime = dbContext.AccountTransactions
            .AsNoTracking()
            .Where(x => x.RecurringTransactionId == recurringTransaction.Id)
            .Max(x => x.DateOccurred);

        var newOccurranceDate = DateOnly.FromDateTime(lastOccurredDateTime);

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
                    return Result.Failure(accountTransactionResult.Error);
                }
            
                var accountTransactionToAdd = accountTransactionResult.Value;
            
                dbContext.AccountTransactions.Add(accountTransactionToAdd);
                try
                {
                    await dbContext.SaveChangesAsync(ct);
                }
                catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    var error = new Error("DBError", $"Error occurred while adding Recurring Item: {ex.Message}");
                    return Result.Failure(error);
                }
            }
        }
        
        
        return Result.Success();
    }


}