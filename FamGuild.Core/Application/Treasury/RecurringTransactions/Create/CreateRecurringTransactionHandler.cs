using FamGuild.Core.Domain.Treasury;

namespace FamGuild.Core.Application.Treasury.RecurringTransactions.Create;

public class CreateRecurringTransactionHandler(FamGuildDbContext dbContext)
    : ICommandHandler<CreateRecurringTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateRecurringTransactionCommand command, CancellationToken ct = default)
    {
        var recurringTransactionResult = RecurringTransaction.Create(
            command.Type.ToString(),
            command.Name,
            command.MoneyAmount,
            command.MoneyCurrency,
            command.Category,
            command.StartDate,
            command.EndDate,
            command.Frequency.ToString()
        );

        if (recurringTransactionResult.IsFailure) return Result.Failure<Guid>(recurringTransactionResult.Error);

        var newRecurringTransaction = recurringTransactionResult.Value;

        dbContext.RecurringTransactions.Add(newRecurringTransaction);
        try
        {
            await dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
        {
            var error = new Error("DBError", $"Error occurred while adding Recurring Item: {ex.Message}");
            return Result.Failure<Guid>(error);
        }

        return Result.Success(newRecurringTransaction.Id);
    }
}