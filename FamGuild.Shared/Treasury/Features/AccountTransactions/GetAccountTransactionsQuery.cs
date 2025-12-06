namespace FamGuild.API.Features.AccountTransactions.Get;

public record GetAccountTransactionsQuery(DateOnly StartDate, DateOnly EndDate);