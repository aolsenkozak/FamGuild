using FamGuild.Core.Application.Common;
using FamGuild.Core.Domain.Common.ResultPattern;
using FamGuild.Core.Domain.Treasury;

namespace FamGuild.Core.Application.Treasury.AccountTransactions.Create;

public class CreateAccountTransactionHandler(FamGuildDbContext dbContext) 
    : ICommandHandler<CreateAccountTransactionsCommand, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> HandleAsync(CreateAccountTransactionsCommand command,
        CancellationToken ct = default)
    {
        var results = command.AccountTransactionsToCreate
            .Select(t => AccountTransaction.Create(
                t.Classification.ToString(),
                t.Name,
                t.MoneyAmount,
                t.MoneyCurrency,
                t.Category,
                t.DateOccurred.ToUniversalTime(),
                t.Status))
            .ToList();
        
        var failure = results.FirstOrDefault(r => r.IsFailure);
        if (failure is not null)
        {
            return Result.Failure<List<Guid>>(failure.Error);
        }

        dbContext.AddRange(results.Select(r => r.Value));
        await dbContext.SaveChangesAsync(ct);

        return Result.Success(results.Select(r => r.Value.Id).ToList());
        
        
    }
}