namespace FamGuild.API.Treasury.AccountTransactions.Get;

public record GetAccountTransactionsQuery(DateOnly StartDate, DateOnly EndDate);