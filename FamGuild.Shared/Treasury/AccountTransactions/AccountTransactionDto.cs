
namespace FamGuild.API.Treasury.AccountTransactions;

public record AccountTransactionDto(Guid Id,
    string Classification,
    string Name,
    decimal MoneyAmount,
    string MoneyCurrency,
    string Category,
    DateTime DateOccurred,
    string  Status,
    Guid? RecurringTransactionId);