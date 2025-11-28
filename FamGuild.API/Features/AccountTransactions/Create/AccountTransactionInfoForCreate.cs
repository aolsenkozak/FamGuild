using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.API.Features.AccountTransactions.Create;

public record AccountTransactionInfoForCreate(
    EntryClassification Classification,
    string Name, 
    Money Amount,
    string Category, 
    DateTime DateOccurred, 
    AccountTransactionStatus Status
    );