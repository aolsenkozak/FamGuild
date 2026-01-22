namespace FamGuild.Core.Application.Treasury.AccountTransactions.Create;

public record CreateAccountTransactionsCommand(
    List<AccountTransactionInfoForCreate> AccountTransactionsToCreate
    );