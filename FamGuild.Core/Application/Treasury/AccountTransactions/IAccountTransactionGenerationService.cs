using FamGuild.Core.Domain.Common.ResultPattern;

namespace FamGuild.Core.Application.Treasury.AccountTransactions;

public interface IAccountTransactionGenerationService
{
    public Task<Result<List<AccountTransactionDto>>> CreateAccountTransactionsForDateRange(
        DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
}