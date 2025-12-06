using FamGuild.Domain.Treasury;
using FamGuild.Domain.Treasury.Common;

namespace FamGuild.API.Features.AccountTransactions;

public record AccountTransactionDto(Guid Id,
    EntryClassification Classification,
    string Name,
    Money Amount,
    string Category,
    DateTime DateOccurred,
    AccountTransactionStatus  Status,
    Guid? RecurringTransactionId);