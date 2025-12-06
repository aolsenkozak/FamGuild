using FamGuild.Domain.Treasury;

namespace FamGuild.API.Features.AccountTransactions;

public static class AccountTransactionMappingExtensions
{
    public static AccountTransactionDto ToDto(this AccountTransaction accountTransaction) =>
        new AccountTransactionDto
            (
        accountTransaction.Id,
        accountTransaction.Classification,
        accountTransaction.Name,
        accountTransaction.Amount,
        accountTransaction.Category,
        accountTransaction.DateOccurred,
        accountTransaction.Status,
        accountTransaction.RecurringTransactionId
        );
}