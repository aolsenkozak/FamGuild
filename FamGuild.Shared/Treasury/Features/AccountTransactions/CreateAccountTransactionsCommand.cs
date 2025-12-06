namespace FamGuild.API.Features.AccountTransactions.Create;

public record CreateAccountTransactionsCommand(
    List<AccountTransactionInfoForCreate> AccountTransactionsToCreate
    );