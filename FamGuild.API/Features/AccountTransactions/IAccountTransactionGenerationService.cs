using FamGuild.API.Domain.Common.ResultPattern;

namespace FamGuild.API.Features.AccountTransactions;

public interface IAccountTransactionGenerationService
{
    public Task<Result<List<AccountTransactionDto>>> CreateAccountTransactionsForDateRange(
        DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
}