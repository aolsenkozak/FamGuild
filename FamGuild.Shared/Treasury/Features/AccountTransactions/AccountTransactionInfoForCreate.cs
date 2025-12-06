using FamGuild.Domain.Treasury;
using FamGuild.Domain.Treasury.Common;

namespace FamGuild.API.Features.AccountTransactions.Create;

public record AccountTransactionInfoForCreate(
    EntryClassification Classification,
    string Name, 
    Money Amount,
    string Category, 
    DateTime DateOccurred, 
    AccountTransactionStatus Status
    );