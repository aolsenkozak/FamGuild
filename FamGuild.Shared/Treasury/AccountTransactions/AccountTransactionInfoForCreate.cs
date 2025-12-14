namespace FamGuild.Shared.Treasury.AccountTransactions;

public record AccountTransactionInfoForCreate(
    string Classification,
    string Name, 
    decimal MoneyAmount,
    string MoneyCurrency,
    string Category, 
    DateTime DateOccurred, 
    string Status
    );