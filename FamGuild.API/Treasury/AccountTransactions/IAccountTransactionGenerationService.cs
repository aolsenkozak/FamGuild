using FamGuild.API.Common.ResultPattern;

namespace FamGuild.API.Treasury.AccountTransactions;

public interface IAccountTransactionGenerationService
{
    public Task<Result<List<AccountTransactionDto>>> CreateAccountTransactionsForDateRange(
        DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
}