namespace FamGuild.API.Features.AccountTransactions.Get;

public record GetAccountTransactionsCommand(DateOnly StartDate, DateOnly EndDate);