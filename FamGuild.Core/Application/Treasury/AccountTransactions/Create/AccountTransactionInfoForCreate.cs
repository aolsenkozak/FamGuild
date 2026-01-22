namespace FamGuild.Core.Application.Treasury.AccountTransactions.Create;

public record AccountTransactionInfoForCreate(
    string Classification,
    string Name, 
    decimal MoneyAmount,
    string MoneyCurrency,
    string Category, 
    DateTime DateOccurred, 
    string Status
    );