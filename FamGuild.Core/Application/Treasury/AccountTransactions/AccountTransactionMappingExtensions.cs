using FamGuild.Core.Domain.Treasury;

namespace FamGuild.Core.Application.Treasury.AccountTransactions;

public static class AccountTransactionMappingExtensions
{
    public static AccountTransactionDto ToDto(this AccountTransaction accountTransaction) =>
        new AccountTransactionDto
            (
        accountTransaction.Id,
        accountTransaction.Classification.ToString(),
        accountTransaction.Name,
        accountTransaction.Amount.Value,
        accountTransaction.Amount.CurrencyCode,
        accountTransaction.Category,
        accountTransaction.DateOccurred,
        accountTransaction.Status.ToString(),
        accountTransaction.RecurringTransactionId
        );
}