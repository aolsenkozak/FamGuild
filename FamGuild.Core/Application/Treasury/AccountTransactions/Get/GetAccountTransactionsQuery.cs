namespace FamGuild.Core.Application.Treasury.AccountTransactions.Get;

public record GetAccountTransactionsQuery(DateOnly StartDate, DateOnly EndDate);