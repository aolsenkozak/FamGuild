namespace FamGuild.Shared.Treasury.AccountTransactions;

public record CreateAccountTransactionsCommand(
    List<AccountTransactionInfoForCreate> AccountTransactionsToCreate
    );